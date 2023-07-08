using AutoMapper;
using Contraacts;
using Entities.Data_Transfer_Object__DTO_.CompanyDTO;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingSystem.ActionFilters;
using TrainingSystem.ModelBinder;

namespace TrainingSystem.Controllers
{
    //to hit a specfic version we either modify the route shape or add querystrings:
    //1-QS=>//hhttps://localhost:7131/api/companies?api-version=2.0
    //2-modify route=> // [Route("api/{v:apiversion}/companies")] => hhttps://localhost:7131/api/2.0/companies

    [ApiVersion("1.0", Deprecated = true)]//set the version as deprecated
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")/*name of swagger doc*/]
    public class CompaniesController : ControllerBase
    {
        private readonly ILoggerManger ILoggerManger;
        private readonly IRepositoryManger IRepositoryManger;
        private readonly IMapper IMapper;


        public CompaniesController(ILoggerManger _ILoggerManger, IRepositoryManger _IRepositoryManger, IMapper _IMapper)
        {
            ILoggerManger = _ILoggerManger;
            IRepositoryManger = _IRepositoryManger;
            IMapper = _IMapper;
        }


        #region 1-CREATE
        //hhttps://localhost:7131/api/companies
        [HttpPost(Name = "Create single Company")]
        //Auto Filtring
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody] POSTCompanyDTO POSTCompanyDTO)
        {


            //The[FromBody] attribute in ASP.NET Core API specifies that the data in the HTTP request body
            //should be bound to the action method parameter. Therefore, the content of the request body
            //will be used to bind to the "POSTCompanyDTO" parameter.

            //This means that any data sent in the request body, such as JSON or XML data, will be
            //deserialized and mapped to the properties of the POSTCompanyDTO object.Other parts
            //of the HTTP request, such as query string parameters or headers, will not be included in
            //the binding process unless they are specifically annotated with other attributes,
            //such as [FromQuery] or[FromHeader].


            //1-
            //Manual Filtering
            //if (POSTCompanyDTO == null)
            //{
            //    ILoggerManger.LogError($"POSTCompanyDTO object set from client is null.");
            //    return BadRequest("POSTCompanyDTO Is null.");
            //}
            //if (!ModelState.IsValid)
            //{
            //    ILoggerManger.LogError($"Invalid ModelState For the POSTCompanyDTO object ");
            //    return UnprocessableEntity(ModelState);
            //}
            //2-
            //map from POSTCompanyDTO to Company
            Company company = IMapper.Map<Company>(POSTCompanyDTO);
            //3-
            //create it and save it to db
            IRepositoryManger.Company.POSTCompany(company);
            await IRepositoryManger.SaveAsync();
            //4-
            //map from Company to CompanyDTO
            GETCompanyDTO GETCompanyDTO = IMapper.Map<GETCompanyDTO>(company);
            //5-
            //The "CreatedAtRoute" method return a response with a location header that contains a URL to the newly created resource.
            //it Creates a CreatedAtRouteResult object that produces a Status201Created in response.
            //in otherwords we create a route(resource / url) to let the user go and see the created company
            //first parameter is The name of the route to use for generating the URL
            //in our case the user can see company details if he hit the GetCompany action,
            //so we gave this action a name to call(copy its route) here
            return CreatedAtRoute("ActionToGetCompanyByID", new { companyId = GETCompanyDTO.Id }, GETCompanyDTO); ;
            //also note the anynomus companyId parameter need to match the route values of the intended action, which mean has same name
            //by this after you post the request => the response directly goes to hit the action ActionToGetCompanyByID givein it
            //our dto obj and its id //in Response Header=> Location : https://localhost:7131/api/Companies/f3aeb9e6-8640-409a-6e65-08db26a70a12

        }


        //hhttps://localhost:7131/api/companies/collection
        [HttpPost("collection")]
        //Auto Filtring
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<POSTCompanyDTO> ListOfPOSTCompanyDTO)
        {
            //Manual Filtering
            //if (ListOfPOSTCompanyDTO == null)
            //{
            //    ILoggerManger.LogError($"ListOfPOSTCompanyDTO object set from client is null.");
            //    return BadRequest("ListOfPOSTCompanyDTO Is null.");
            //}
            //if (!ModelState.IsValid)
            //{
            //    ILoggerManger.LogError($"Invalid ModelState For the POSTCompanyDTO object ");
            //    return UnprocessableEntity(ModelState);
            //}

            IEnumerable<Company> AllCompinesByIds = IMapper.Map<IEnumerable<Company>>(ListOfPOSTCompanyDTO);

            foreach (var company in AllCompinesByIds) IRepositoryManger.Company.POSTCompany(company);

            await IRepositoryManger.SaveAsync();


            IEnumerable<GETCompanyDTO> AllCompinesByIdsDTO = IMapper.Map<IEnumerable<GETCompanyDTO>>(AllCompinesByIds);

            string CompaniesIds = string.Join(",", AllCompinesByIdsDTO.Select(GETCompanyDTO => GETCompanyDTO.Id));

            return CreatedAtRoute("ActionOfGetCompanyCollection", new { CompaniesIds }, AllCompinesByIdsDTO);
            //the final url of Location at header from CreateCompanyCollection action =>
            //hhttps://localhost:7131/api/Companies/collection/(b79c8281-1f82-4e13-d60f-08db26c5f80b,e396f2d2-ccfd-4ebc-d610-08db26c5f80b)

        }
        #endregion


        #region 2-READ
        /// <summary>
        /// Get the full list of Companies ystaa
        /// </summary>
        /// <returns>The Companies list</returns>
        //hhttps://localhost:7131/api/companies
        [HttpGet(Name = "Get All Companeis"), Authorize]
        public async Task<IActionResult> GetCompanies()
        {

            IEnumerable<Company> CompinesList = await IRepositoryManger.Company.GetAllCompinesAsync(trackChanges: false);
            //return Ok(CompinesList);


            //Manually mapping properties from Company object to GETCompanyDTO object
            IEnumerable<GETCompanyDTO> ListOfGETCompanyDTO = CompinesList.Select(Company =>
            new GETCompanyDTO
            {
                Id = Company.Id,
                Name = Company.Name,
                FullAdress = string.Join(' ', Company.Adress, ' ', Company.Country)
            }).ToList();
            //return Ok(CompinesDTOList);

            //AutoMapper properties from Company object to GETCompanyDTO object
            IEnumerable<GETCompanyDTO> ListOfGETCompanyDTOByAutoMapper = IMapper.Map<IEnumerable<GETCompanyDTO>>(CompinesList);
            return Ok(ListOfGETCompanyDTOByAutoMapper);

        }


        //hhttps://localhost:7131/api/companies/92f04823-3b8f-48c4-a37f-32529f8724f5/
        [HttpGet("{companyId}", Name = "ActionToGetCompanyByID")]
        //Auto Filtring
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> GetCompany(Guid companyId)
        {
            //Manual Filtering
            Company company2 = await IRepositoryManger.Company.GetCompanyByIdAsync(companyId, trackChanges: false);//we not comment this line just to avoid CS1998 
            //if(company == null)
            //{
            //    ILoggerManger.LogWarning($"Company with the ID: {companyId} doesn't exist in the database");
            //    return NotFound();
            //}
            Company company = HttpContext.Items["company"] as Company;

            GETCompanyDTO GETCompanyDTO = IMapper.Map<GETCompanyDTO>(company);
            return Ok(GETCompanyDTO);
        }




        //the final url of Location at header from CreateCompanyCollection action =>
        //hhttps://localhost:7131/api/Companies/collection/(b79c8281-1f82-4e13-d60f-08db26c5f80b,e396f2d2-ccfd-4ebc-d610-08db26c5f80b)
        //by this url the ActionOfGetCompanyCollection will not understand (b79c8281-1f82-4e13-d60f-08db26c5f80b,e396f2d2-ccfd-4ebc-d610-08db26c5f80b)
        //because he excepect list of  ids => IEnumerable<Guid> CompaniesIds, not (id1,id2)
        //so we will create ArrayModelBinder class to bind (id1,id2) by [ModelBinder(binderType:typeof(ArrayModelBinder))]IEnumerable<Guid> CompaniesIds
        //so when this action hit, firstly ArrayModelBinder hit and resolve the incoming (id1,id2)
        //and map it to valid values which understood by the ActionOfGetCompanyCollection
        [HttpGet("collection/({CompaniesIds})", Name = "ActionOfGetCompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(binderType: typeof(ArrayModelBinder))] IEnumerable<Guid> CompaniesIds)
        {
            if (CompaniesIds == null)
            {
                ILoggerManger.LogError($"CompaniesIds parameter  is null.");
                return BadRequest("CompaniesIds parameter Is null.");
            }
            IEnumerable<Company> AllCompinesByIds = await IRepositoryManger.Company.GetAllCompinesByIdsAsync(CompaniesIds, trackChanges: false);
            if (CompaniesIds.Count() != AllCompinesByIds.Count())
            {
                ILoggerManger.LogError($"Some Ids not valid in the collection");
                return NotFound();
            }
            IEnumerable<GETCompanyDTO> AllCompinesByIdsDTO = IMapper.Map<IEnumerable<GETCompanyDTO>>(AllCompinesByIds);
            return Ok(AllCompinesByIdsDTO);

        }



        #endregion



        #region 3-UPDATE

        //hhttps://localhost:7131/api/companies/92f04823-3b8f-48c4-a37f-32529f8724f5/
        [HttpPut("{companyId}")]
        //Auto Filtring
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]

        public async Task<IActionResult> UpdateCompany(Guid companyId, [FromBody] UPDATECompanyDTO UPDATECompanyDTO)
        {
            //Manual Filtering
            //if (UPDATECompanyDTO == null)
            //{
            //    ILoggerManger.LogError($"UPDATECompanyDTO object set from client is null.");
            //    return BadRequest("UPDATECompanyDTO Is null.");
            //}
            //if (!ModelState.IsValid)
            //{
            //    ILoggerManger.LogError($"Invalid ModelState For the UPDATECompanyDTO object ");
            //    return UnprocessableEntity(ModelState);
            //}

            //Manual Filtering
            //Company company = await IRepositoryManger.Company.GetCompanyByIdAsync(companyId, trackChanges: true);
            //if (company == null)
            //{
            //    ILoggerManger.LogWarning($"Company with the ID: {companyId} doesn't exist in the database");
            //    return NotFound();
            //}
            Company company = HttpContext.Items["companyEntity"] as Company;
            IMapper.Map(UPDATECompanyDTO, company);
            await IRepositoryManger.SaveAsync();
            return NoContent();

        }

        #endregion


        #region 4-DELETE

        //hhttps://localhost:7131/api/companies/92f04823-3b8f-48c4-a37f-32529f8724f5
        [HttpDelete("{companyId}")]
        //Auto Filtring
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteCompany(Guid companyId)
        {
            //Manual Filtering
            //Company company = await IRepositoryManger.Company.GetCompanyByIdAsync(companyId, trackChanges: false);
            //if (company == null)
            //{
            //    ILoggerManger.LogWarning($"Company with the ID: {companyId} doesn't exist in the database");
            //    return NotFound();
            //}

            //retrive the company object we insert on our HttpContext items in our filtr attribute
            Company company = HttpContext.Items["company"] as Company;

            //note that we delete the company and its contained employees ,this viloate rule of FK cascading refential integrity, but busniess is busniess
            IRepositoryManger.Company.DeleteCompany(company);
            await IRepositoryManger.SaveAsync();
            return NoContent();//204
        }
        #endregion



        #region 5- HTTP OPTION Method
        //hhttps://localhost:7131/api/companies
        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,ABDO");
            return Ok();
        }
        #endregion

    }
}
