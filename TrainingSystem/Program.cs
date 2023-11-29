using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Web;
using TrainingSystem.ActionFilters;
using TrainingSystem.Extensions;





NLog.Logger logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("start app");


try
{
    #region Create importantt Instances 
    WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);



    //create serviceProvider instance
    ServiceProvider serviceProvider = webApplicationBuilder.Services.BuildServiceProvider();

    //create IConfiguration instance
    //IConfiguration iConfiguration = serviceProvider.GetRequiredService<IConfiguration>();
    var configuration = webApplicationBuilder.Configuration;


    //create IWebHostEnvironment instance
    IWebHostEnvironment iWebHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();

    //create IServiceCollection instance
    IServiceCollection iServiceCollection = webApplicationBuilder.Services;

    #endregion




    #region Register Services

    //1-Cors
    iServiceCollection.RegisterCorsOrigin();

    //2-logger
    iServiceCollection.RegisterLoggerService();

    //3-Api Versioning
    iServiceCollection.RegisterApiVersioning();

    //4-automapper
    iServiceCollection.AddAutoMapper(typeof(Program).Assembly);

    //5-IIS
    iServiceCollection.RegisterIISIntegration();

    //6-RepoManger 
    iServiceCollection.RegisterRepositoryManger();

    //7-Caching
    iServiceCollection.RegisterCaching();
    iServiceCollection.RegisterHTTPHeaderCaching();

    //12-swager
    iServiceCollection.ConfigureSwagger();


    //8-Global Action Filtring
    iServiceCollection.AddScoped<ValidationFilterAttribute>();

    iServiceCollection.AddScoped<ValidateCompanyExistsAttribute>();

    iServiceCollection.AddScoped<ValidateEmployeeForCompanyExistsAttribute>();

    //9-Identity
    iServiceCollection.AddAuthentication();
    iServiceCollection.RegisterIdentity();

    //10-JWT
    iServiceCollection.ConfigureJWT(configuration);

    //9-support validation filters and model state
    iServiceCollection.Configure<ApiBehaviorOptions>(ApiBehaviorOptions =>
    {
        ApiBehaviorOptions.SuppressModelStateInvalidFilter = true;
    });

    //10-sql context
    iServiceCollection.RegisterSQLContext(configuration);


    //11-API
    iServiceCollection.AddControllers(MvcOptions =>
        {
            MvcOptions.RespectBrowserAcceptHeader = true;//take in consider the information in the acceptHeader
            MvcOptions.ReturnHttpNotAcceptable = true; //return 406  for not supported content nogation for you app

            MvcOptions.CacheProfiles.Add("TrainingSystem_cach_profile_name:131SecondsDuration", new CacheProfile  //support cache for controller level   
            {
                Duration = 131
            });

        })
        .AddNewtonsoftJson()//support patch method by add the NewtonsoftJson
        .AddXmlDataContractSerializerFormatters();//support XML















    #endregion




    var app = webApplicationBuilder.Build();




    #region Configure the HTTP request pipeline.


    //1-error handling
    if (app.Environment.IsDevelopment())
    {

        app.UseDeveloperExceptionPage();
    }
    //2-global error handling
    app.ConfigureExceptionHandling(logger);


    //3-redirection
    app.UseHttpsRedirection();

    //4-Static Files
    app.UseStaticFiles();

    //5-cors
    app.UseCors("CorsPolicy");

    //6-Caching before routing
    app.UseResponseCaching(); //by this we add the cache to response body, which add the age haeader in response headers, this age specifiy how old this resource, and when resource age >= maxage value in cash-control => the resource need to be cashed again
    app.UseHttpCacheHeaders();//override  UseResponseCaching

    //7-Swagger
    app.UseSwagger();
    app.UseSwaggerUI(SwaggerUIOptions =>
    {

        SwaggerUIOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "TrainingSystem Apis v1 by swagger");//v1 is SwaggerDocumentationNumberOne
        SwaggerUIOptions.SwaggerEndpoint("/swagger/v2/swagger.json", "TrainingSystem Apis v2 by swagger");//v2 is SwaggerDocumentationNumberOne
    });

    //8-Routing
    app.UseRouting();

    //9-Authentication
    app.UseAuthentication();

    //10-Authorization
    app.UseAuthorization();

    //11-endpoint
    app.UseEndpoints(iEndpointRouteBuilder =>
    {
        iEndpointRouteBuilder.MapControllers();
        //iEndpointRouteBuilder.MapControllerRoute(
        //    name:"The default Route",
        //    pattern: "{controller=Companies}/{action=GetCompanies}/{id?}"); 
    });

    app.Run();
    #endregion


}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception catched by nlog");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}