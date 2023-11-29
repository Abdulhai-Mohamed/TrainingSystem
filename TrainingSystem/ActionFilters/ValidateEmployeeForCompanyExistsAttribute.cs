using Contraacts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TrainingSystem.ActionFilters
{
    public class ValidateEmployeeForCompanyExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManger IRepositoryManger;
        private readonly ILoggerManger ILoggerManger;
        public ValidateEmployeeForCompanyExistsAttribute(IRepositoryManger repository, ILoggerManger logger)
        {
            IRepositoryManger = repository;
            ILoggerManger = logger;
        }
        //Our async code before action excutes
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string method = context.HttpContext.Request.Method;
            //we check a type of request and only if it is a PUT/PATCH request we set the trackChanges to true
            bool trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;


            Guid companyId = (Guid)context.ActionArguments["companyId"];
            Company company = await IRepositoryManger.Company.GetCompanyByIdAsync(companyId, trackChanges: true);
            if (company == null)
            {
                ILoggerManger.LogWarning($"Company with the ID: {companyId} doesn't exist in the database");
                context.Result = new NotFoundResult();
                return;

            }

            Guid EmployeeId = (Guid)context.ActionArguments["EmployeeId"];


            Employee employee = await IRepositoryManger.Employee.GetEmployeeByIdAsync(companyId, EmployeeId, trackChanges);
            if (employee == null)
            {
                ILoggerManger.LogWarning($"employee with the ID: {EmployeeId} doesn't exist in the database");
                context.Result = new NotFoundResult();
            }
            else
            {
                //Adds an element with the provided key and value to the System.Collections.Generic.IDictionary`2.

                //2-
                //if we find the entity in the database(!=null), we store it in HttpContext because we need that
                //entity in our action methods and we don’t want to query the database two times
                //(we would lose more than we gain if we double that action)
                context.HttpContext.Items.Add("employeeEntity", employee);
                await next();
                //3-register this filter in program
            }
        }
    }
}
