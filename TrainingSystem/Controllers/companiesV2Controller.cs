using AutoMapper;
using Contraacts;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingSystem.ActionFilters;

namespace TrainingSystem.Controllers
{
    //to hit a specfic version we either modify the route shape or add querystrings:
    //1-QS=>//hhttps://localhost:7131/api/companies?api-version=2.0
    //2-modify route=> // [Route("api/{v:apiversion}/companies")] => hhttps://localhost:7131/api/2.0/companies

    [ApiVersion("2.0")]
    [Route("api/companies")]
    [ApiController]
    [ResponseCache(CacheProfileName = "abdo_cach_profile_name:131SecondsDuration")]//add specifc cach profile name for companies controller level
    [ApiExplorerSettings(GroupName = "v2")/*name of swagger doc*/]

    public class companiesV2Controller : ControllerBase
    {
        private readonly IRepositoryManger IRepositoryManger;


        public companiesV2Controller(IRepositoryManger _IRepositoryManger)
        {
            IRepositoryManger = _IRepositoryManger;
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            IEnumerable<Company> CompinesList = await IRepositoryManger.Company.GetAllCompinesAsync(trackChanges: false);
            return Ok(CompinesList);


        }


        //hhttps://localhost:7131/api/companies/92f04823-3b8f-48c4-a37f-32529f8724f5/
        [HttpGet("{companyId}")]
        //Auto Filtring
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        [ResponseCache(Duration=150)]//set this resource as cachable resource and its duration is 150(override 131 of the controller) , this just add the header Cach-Control and assign 150 to its maxage(duration) => Cache-Control: public,max-age=150

        //another way to add cach by marvin librarry,note that it will not work if you not register AddHttpCacheHeaders and configure  UseHttpCacheHeaders
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 170)] //override 150
        [HttpCacheValidation(MustRevalidate = true)]

        public async Task<IActionResult> GetCompany(Guid companyId)
        {
            Company company = await IRepositoryManger.Company.GetCompanyByIdAsync(companyId, trackChanges: false);//we not comment this line just to avoid CS1998 
            return Ok(company);
        }
    }
}
