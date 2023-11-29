using Contraacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel;

namespace TrainingSystem.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        private readonly ILoggerManger ILoggerManger;

        public ValidationFilterAttribute(ILoggerManger _ILoggerManger) 
        {
            ILoggerManger = _ILoggerManger;
        }

    
        //Our code before action excutes
        public void OnActionExecuting(ActionExecutingContext ActionExecutingContext)
        {
            //1-get the controller
           var controller = ActionExecutingContext.RouteData.Values["controller"];//"Companies"
            //2-get the action
            var Action = ActionExecutingContext.RouteData.Values["action"];//"CreateCompany"
            //3-get the DTO object which be checked, 
            //this mean work for dto object just
            //and here we check the object that the action take is its name contain dto or not, this mean we not chechk the objcet sent by post man because basically all 
            //objects are the same which it is a json object and have no names
            //so value of DTOObjectTobeChecked will be null if the action not take any parameter object contain dto in its name 
            var DTOObjectTobeChecked = ActionExecutingContext.ActionArguments.
                SingleOrDefault(x=>x.Value.ToString().Contains("DTO")).Value;
            //{[POSTCompanyDTO, {Entities.Data_Transfer_Object__DTO_.CompanyDTO.POSTCompanyDTO}]}

            //{ Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<Entities.Data_Transfer_Object__DTO_.EmployeeDTO.UPDATEEmployeeDTO>}


            //4-check nullable
            if (DTOObjectTobeChecked == null) 
            {
                ILoggerManger.LogError($"from global filtring DTOObjectTobeChecked  set from client is null.\nController is: {controller} amd its action: {Action}");
                ActionExecutingContext.Result = new BadRequestObjectResult($"DTOObjectTobeChecked is null .\nController is: {controller} amd its action: {Action}");
                return;
            }

            //5-check validation
            if(!ActionExecutingContext.ModelState.IsValid)
            {
                ILoggerManger.LogError($"from global filtring DTOObjectTobeChecked  set from client is null.\nController is: {controller} amd its action: {Action}");
                ActionExecutingContext.Result = new UnprocessableEntityObjectResult(ActionExecutingContext.ModelState);

            }

        }

        //Our code after action excutes
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
