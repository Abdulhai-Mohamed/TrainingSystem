using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Contraacts;
using Entities.Models;
using System;

namespace TrainingSystem.ActionFilters
{
    public class ValidateCompanyExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManger IRepositoryManger;
        private readonly ILoggerManger ILoggerManger;
        public ValidateCompanyExistsAttribute(IRepositoryManger repository,ILoggerManger logger)
        {
            IRepositoryManger = repository;
            ILoggerManger = logger;
        }

        //Our async code before action excutes
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //1-
            //we check a type of request and only if it is a PUT request we set the trackChanges to true
            bool trackChanges = context.HttpContext.Request.Method.Equals("PUT");
            
            Guid companyId = (Guid)context.ActionArguments["companyId"];
            Company company = await IRepositoryManger.Company.GetCompanyByIdAsync(companyId, trackChanges);
            if (company == null)
            {
                ILoggerManger.LogWarning($"Company with the ID: {companyId} doesn't exist in the database");
                context.Result = new NotFoundResult();
            }
            else
            {
                //Adds an element with the provided key and value to the System.Collections.Generic.IDictionary`2.

                //2-
                //if we find the entity in the database(!=null), we store it in HttpContext because we need that
                //entity in our action methods and we don’t want to query the database two times
                //(we would lose more than we gain if we double that action)
                context.HttpContext.Items.Add("company", company);
                await next();
                //3-register this filter in program

            }
        }
    }
    
}
