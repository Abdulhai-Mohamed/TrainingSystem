using Contraacts;
using Entities;
using Entities.Models;
using LoggingService;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using System.Reflection;
using System.Text;
using TrainingSystem.Utility;
namespace TrainingSystem.Extensions
{
    public static class ServiceExtensions
    {
        //1-cors
        public static void RegisterCorsOrigin(this IServiceCollection services)
        {

            services.AddCors(CorsOptions =>
            {
                CorsOptions.AddPolicy("CorsPolicy", CorsPolicyBuilder =>
                {
                    //CorsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowCredentials().AllowAnyHeader() //System.InvalidOperationException: 'The CORS protocol does not allow specifying a wildcard (any) origin and credentials at the same time. Configure the CORS policy by listing individual origins if credentials needs to be supported.'
                    CorsPolicyBuilder.WithOrigins("http://localhost:8080")
.AllowAnyMethod().AllowCredentials().AllowAnyHeader();
                });
            });

        }

        //2-IIS
        public static void RegisterIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

                //...................
            });
        }


        //3-Logging
        public static void RegisterLoggerService(this IServiceCollection services)
        {
            services.AddScoped<ILoggerManger, LoggerManger>();
        }

        //4-sql dbcontext
        public static void RegisterSQLContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContextPool<RepositoryContext>(DbContextOptionsBuilder =>
            {
                DbContextOptionsBuilder.UseSqlServer
                    (
                        config.GetConnectionString("SqlConnection"),
                        //get the namespace of classes which will be mapped to db
                        SqlServerDbContextOptionsBuilder => SqlServerDbContextOptionsBuilder.MigrationsAssembly("TrainingSystem")
                    );
            });
        }

        //5-RepoManger 
        public static void RegisterRepositoryManger(this IServiceCollection services)
        {
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IRepositoryManger, RepositoryManger>();
        }


        //6-Register Versioning Service
        public static void RegisterApiVersioning(this IServiceCollection services)
        {


            services.AddApiVersioning(ApiVersioningOptions =>
            {
                ApiVersioningOptions.DefaultApiVersion = new ApiVersion(1, 0);//set the default version
                ApiVersioningOptions.AssumeDefaultVersionWhenUnspecified = true;//enable a default version
                ApiVersioningOptions.ReportApiVersions = true;//add the version in response header => api-supported-version:   , also note that if the version is set as deprecated => the header will be api-deprecaated-version: 
                                                              //ApiVersioningOptions.ApiVersionReader = new HeaderApiVersionReader("api-version"); //this option make querystring versioning not wor


            })
            //for swagger
            .AddVersionedApiExplorer(ApiExplorerOptions =>
            {
                ApiExplorerOptions.GroupNameFormat = "'v'VVV";
                ApiExplorerOptions.SubstituteApiVersionInUrl = true;

            });

        }
        //7-Register Caching Service
        //this is first mechanisim of http cashing response => Expiraty approach by  age header
        public static void RegisterCaching(this IServiceCollection services) =>
            services.AddResponseCaching();

        //8-Register HTTP Header Caching Service
        //this is second mechanisim of http cashing response => Validation approach by  many headers like [ Expires, Last-Modified, ETag, Vary ]
        public static void RegisterHTTPHeaderCaching(this IServiceCollection services)
        {

            //register both expiraty and validation mechanisim by ine method:
            services.AddHttpCacheHeaders  //override  AddResponseCaching
            (
                (ExpirationModelOptions) =>
                {
                    ExpirationModelOptions.MaxAge = 206;
                    ExpirationModelOptions.CacheLocation = CacheLocation.Private;//the cach doen in browser
                },
                (ValidationModelOptions) =>
                {
                    ValidationModelOptions.MustRevalidate = true;
                    //to validate the cached resource and check if it is modified or not
                    //we use the value of ETag header and set it as value to the querystring if-none-match = > 
                    //if status code is 304 this mean the resource not modified 
                }
            );

        }



        //9-Identity
        public static void RegisterIdentity(this IServiceCollection services)
        {
            IdentityBuilder builder = services.AddIdentityCore<ApplicationUser>(IdentityOptions =>
            {

                IdentityOptions.User.RequireUniqueEmail = true;
                IdentityOptions.Password.RequireDigit = true;
                IdentityOptions.Password.RequireLowercase = false;
                IdentityOptions.Password.RequireUppercase = false;
                IdentityOptions.Password.RequireNonAlphanumeric = false;
                IdentityOptions.Password.RequiredLength = 8;
            });
            //This initial IdentityBuilder instance is used to configure various options for the
            //Identity service, such as password complexity requirements.
            //However, in order to fully register the Identity service with the dependency injection
            //container, a new IdentityBuilder instance must be created that includes both the user model
            //and the role model. That's where the second IdentityBuilder instance comes in.
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<RepositoryContext>()
                .AddDefaultTokenProviders();




        }


        //10-JWT 
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration IConfiguration)
        {
            //1-
            services.AddScoped<IAuthenticationManger, AuthenticationManger>();

            //2-
            IConfiguration JWtSettings = IConfiguration.GetSection("JWtSettings");
            string secretKeyFromEnvVariables = Environment.GetEnvironmentVariable("SECRET");//TrainingSystemSecretJWTTokensKeyForTrainingSystem

            //var z = JWtSettings.GetSection("validIssuer").Value;
            //var xx = JWtSettings.GetSection("validAudience").Value;
            services
                .AddAuthentication(AuthenticationOptions =>
                {
                    AuthenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    AuthenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerOptions =>
                {
                    JwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = JWtSettings.GetSection("validIssuer").Value,
                        ValidateAudience = true,
                        ValidAudience = JWtSettings.GetSection("validAudience").Value,
                        ValidateLifetime = true,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyFromEnvVariables))//encode secretkey to array of bytes
                        //for deploy
                        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AbdoSecretJWTTokensKeyForTrainingSystem"))//encode secretkey to array of bytes
                    };
                });



        }


        //11-swagger
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            //services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(SwaggerGenOptions =>
            {

                //you shoud create a swagger documentation foreach version of your APIS(in case of you have many versions)
                //1-docmentaion1
                SwaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo //v1 is SwaggerDocumentationNumberOne
                {
                    Title = "TrainingSystemApis v1",
                    Version = "version: 1",
                    Description = "Your API Description",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Abdul-Hai",
                        Email = "ABDULHAI.MOHAMED.SAMY@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/abdul-hai-mohamed-158205219/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Training System Api LICENSE",
                        Url = new Uri("https://example.com/license")
                    }

                });
                //1-docmentaion2
                SwaggerGenOptions.SwaggerDoc("v2", new OpenApiInfo //v2 is SwaggerDocumentationNumberOne
                {
                    Title = "TrainingSystemApis v2",
                    Version = "version: 2",
                    Description = "Your API Description",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Abdul-Hai",
                        Email = "ABDULHAI.MOHAMED.SAMY@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/abdul-hai-mohamed-158205219/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Training System Api LICENSE",
                        Url = new Uri("https://example.com/license")
                    }
                });



                //2-
                //The code block is responsible for including XML comments in the Swagger documentation generated for the project.
                //The comments are generated from the code comments added by the developer to classes, methods, and properties in the code.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";//name of the XML file that contains the XML comments,The name of the XML file is based on the name of the executing assembly, which is the assembly that contains the code being executed.
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);//represents the full path of the XML file
                SwaggerGenOptions.IncludeXmlComments(xmlPath);//the IncludeXmlComments method on the SwaggerGenOptions object to include the XML comments in the Swagger documentation. The method takes the full path of the XML file as a parameter. The XML comments are then used to generate the descriptions for the API endpoints and models in the Swagger documentation.

                //3-
                //AddSecurityDefinition() is a method used to define a security scheme that can be used to secure your API endpoints in Swagger.
                //The scheme is used to authenticate requests using JSON Web Tokens (JWTs) by placing the token in the Authorization header of the request with the "Bearer" prefix.

                SwaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme // Bearer: The name of the security scheme. This is the name that will be used when defining security requirements for individual operations in the API.  and it is defined as a type of API Key
                {//OpenApiSecurityScheme: A class representing the details of the security scheme being defined.
                    In = ParameterLocation.Header,//In: The location of the token in the request. In this case, the token is expected to be in the "Authorization" header.
                    Name = "Authorization",//The name of the parameter that contains the token.
                    Description = "Place to add JWT with Bearer",// A description of the security scheme.
                    Scheme = "Bearer",//The name of the authentication scheme. In this case, "Bearer" is used to indicate that JWTs should be used for authentication.
                    Type = SecuritySchemeType.ApiKey //The type of the security scheme. In this case, the type is set to "ApiKey" to indicate that a token is being used for authentication.
                });

                //4-
                //After defining the security scheme using AddSecurityDefinition(), you can then apply it to individual operations or to the entire API using SwaggerGenOptions.AddSecurityRequirement(). This will require that any requests to the API include a valid JWT in the Authorization header with the "Bearer" prefix.
                SwaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {


                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()


                    }
                });

            });
        }

    }
}
