using Contraacts;
using Entities.ErroModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Runtime.CompilerServices;

namespace TrainingSystem.Extensions
{
    public static class ExceptionMiddlewareExtension
    {

        public static void ConfigureExceptionHandling(this IApplicationBuilder app, NLog.Logger logger) 
        {
            //Global exception handling:
            // Adds a middleware to the pipeline that will catch exceptions, log them, and re-execute the request in an alternate pipeline.
            // The request will not be re-executed if the response has already started.

            //app.UseExceptionHandler("/Error");//redirect the request to error controller
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async HttpContext =>
                {
                    //1-set the status code to 500
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    //2-set the response as json not any other types like xml or text
                    HttpContext.Response.ContentType = "application/json";

                    //3-
                    //Gets the collection of HTTP features provided by the server and middleware available on this request,
                    //then Retrieves the requested feature from the collection,
                    //here wewaant retreive IExceptionHandlerFeature which represent all the errors in the requesst
                    var contextExceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
                    if (contextExceptionFeature != null)
                    {
                        logger.Error($"from gloal exception handling\n an error occured:\n{contextExceptionFeature.Error}");
                        await HttpContext.Response.WriteAsync(new ErrorDetails
                        {
                            StatusCode = HttpContext.Response.StatusCode,
                            Message = "INTERNAL SERVER ERROR YAMO7TERM"
                        }.ToString());
                    }

                });
            });
        
        
        }
    }
}
