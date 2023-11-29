using AutoMapper;
using Contraacts;
using Entities.Data_Transfer_Object__DTO_.EmployeeDTO;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TrainingSystem.ActionFilters;

namespace TrainingSystem.Controllers
{
    [ApiVersion("2.0")]

    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")/*name of swagger doc*/]

    public class EmployeesV2Controller : ControllerBase
    {
        private readonly ILoggerManger ILoggerManger;
        private readonly IRepositoryManger IRepositoryManger;
        private readonly IMapper IMapper;


        public EmployeesV2Controller(ILoggerManger _ILoggerManger, IRepositoryManger _IRepositoryManger, IMapper _IMapper)
        {
            ILoggerManger = _ILoggerManger;
            IRepositoryManger = _IRepositoryManger;
            IMapper = _IMapper;

        }



        #region 2-READ
        //hhttps://localhost:7131/api/companies/92f04823-3b8f-48c4-a37f-32529f8724f5/employess === 
        //  
        //hhttps://localhost:7131/api/companies/92f04823-3b8f-48c4-a37f-32529f8724f5/employees?pageNumber=1&pageSize=50&MaxAge=100&MinAge=1&searchTerm=sara

        [HttpGet]
        [HttpHead]//used to mark an action method that should respond to HTTP HEAD requests.//HTTP HEAD is a request method that is similar to HTTP GET, but instead of returning a full response body, it only returns the HTTP headers of the requested resource. This can be useful in situations where a client needs to know information about a resource, such as its size or modification date, without actually downloading the resource itself.
        //Auto Filtring
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> GetAllEmplyessOfCompany(Guid companyId, [FromQuery] EmployeeParameters EmployeeParameters)
        {
            //Manual Filtering
            //Company company = await IRepositoryManger.Company.GetCompanyByIdAsync(companyId, trackChanges: false);
            //if (company == null)
            //{
            //    ILoggerManger.LogWarning($"Company with the ID: {companyId} doesn't exist in the database");
            //    return NotFound();
            //}
            if (!EmployeeParameters.ValidRange) return BadRequest("Max age can't be less than min age");

            PagedList<Employee> AllEmplyessOfCompany = await IRepositoryManger.Employee.GetEmployeesAsync(companyId, EmployeeParameters, trackChanges: false);
            //return Ok(AllEmplyessOfCompany);

            Response.Headers.Add("Pagination-Metadata", JsonConvert.SerializeObject(AllEmplyessOfCompany.PaginationMetaData));


            IEnumerable<GETEmployeeDTO> AllDTOEmplyessOfCompany = IMapper.Map<IEnumerable<GETEmployeeDTO>>(AllEmplyessOfCompany);
            return Ok(AllDTOEmplyessOfCompany);
        }




        //hhttps://localhost:7131/api/companies/92f04823-3b8f-48c4-a37f-32529f8724f5/employess/5134eb68-3a7e-4042-a549-b92b87abbc03
        [HttpGet("{EmployeeId}", Name = "ActionToGetEmployeeByID")]
        //Auto Filtring
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> GetEmployee(Guid companyId, Guid EmployeeId)
        {
            //Manual Filtering
            Company company2 = await IRepositoryManger.Company.GetCompanyByIdAsync(companyId, trackChanges: false);//we not comment this line just to avoid CS1998 
            //if (company == null)
            //{
            //    ILoggerManger.LogWarning($"Company with the ID: {companyId} doesn't exist in the database");
            //    return NotFound();
            //}
            //Employee employee = await IRepositoryManger.Employee.GetEmployeeByIdAsync(companyId, EmployeeId, trackChanges: false);
            //if (employee == null)
            //{
            //    ILoggerManger.LogWarning($"employee with the ID: {EmployeeId} doesn't exist in the database");
            //    return NotFound();
            //}


            Employee employee = HttpContext.Items["employeeEntity"] as Employee;

            GETEmployeeDTO GETEmployeeDTO = IMapper.Map<GETEmployeeDTO>(employee);
            return Ok(GETEmployeeDTO);


        }
        #endregion




        #region 1-CREATE
        //hhttps://localhost:7131/api/companies/92f04823-3b8f-48c4-a37f-32529f8724f5/employess/
        [HttpPost]
        //Auto Filtring
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] POSTEmployeeDTO POSTEmployeeDTO)
        {
            //Manual Filtering
            //if (POSTEmployeeDTO == null)
            //{
            //    ILoggerManger.LogError($"POSTEmployeeDTO object set from client is null.");
            //    return BadRequest("POSTEmployeeDTO Is null.");
            //}
            //if (!ModelState.IsValid)
            //{
            //    ILoggerManger.LogError($"Invalid ModelState For the POSTEmployeeDTO object ");
            //    return UnprocessableEntity(ModelState);
            //}

            Company company = await IRepositoryManger.Company.GetCompanyByIdAsync(companyId, trackChanges: false);
            if (company == null)
            {
                ILoggerManger.LogWarning($"Company with the ID: {companyId} doesn't exist in the database");
                return NotFound();
            }
            Employee Employee = IMapper.Map<Employee>(POSTEmployeeDTO);
            IRepositoryManger.Employee.POSTEmployeeForCompany(companyId, Employee);
            await IRepositoryManger.SaveAsync();

            GETEmployeeDTO GETEmployeeDTO = IMapper.Map<GETEmployeeDTO>(Employee);
            return CreatedAtRoute("ActionToGetEmployeeByID", new { companyId, EmployeeId = GETEmployeeDTO.Id }, GETEmployeeDTO); ;
            //our dto obj and its id //in Response Header=>Location:https://localhost:7131/api/companies/455cdf8b-9469-45e4-aa05-0c24b3541503/employess/49223ce9-2a1a-40d0-ee79-08db26b33c44
        }
        #endregion



    }


}
