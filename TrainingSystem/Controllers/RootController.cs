using Entities.LinksModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//here we manually do what swagger do 
namespace TrainingSystem.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        private readonly LinkGenerator LinkGenerator;
        public RootController(LinkGenerator _LinkGenerator)
        {
            LinkGenerator = _LinkGenerator;
        }

        [HttpGet(Name ="GetRoot")]
        public IActionResult GetRoot()
        {
            List<Link> linkList = new List<Link>()
            {
                new Link()
                {
                    Href = LinkGenerator.GetUriByName(HttpContext,nameof(GetRoot),new{ }),
                    Rel = "self" , Method = "GET"
                },
                new Link()
                {
                    Href = LinkGenerator.GetUriByName(HttpContext,"Get All Companeis",new{ }),
                    Rel = "companies" , Method = "GET"
                },
                new Link()
                {
                    Href = LinkGenerator.GetUriByName(HttpContext,"Create single Company",new{ }),
                    Rel = "companies" , Method = "Post"
                }
            };
            return Ok(linkList);
            /*
             Documented output:
            [
                {
                    "href": "https://localhost:7131/api",
                    "rel": "self",
                    "method": "GET"
                },
                {
                    "href": "https://localhost:7131/api/Companies",
                    "rel": "companies",
                    "method": "GET"
                },
                {
                    "href": "https://localhost:7131/api/Companies",
                    "rel": "companies",
                    "method": "Post"
                }
            ]
             
             */
        }


    }
}
