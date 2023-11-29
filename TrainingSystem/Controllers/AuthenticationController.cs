using AutoMapper;
using Contraacts;
using Entities.Data_Transfer_Object__DTO_.ApplicationUserDTO;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using TrainingSystem.ActionFilters;

namespace TrainingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoggerManger ILoggerManger;
        private readonly IRepositoryManger IRepositoryManger;
        private readonly IMapper IMapper;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IAuthenticationManger IAuthenticationManger;

        public AuthenticationController(ILoggerManger _ILoggerManger, IRepositoryManger _IRepositoryManger, IMapper _IMapper, UserManager<ApplicationUser> _UserManager, IAuthenticationManger _IAuthenticationManger)
        {
            ILoggerManger = _ILoggerManger;
            IRepositoryManger = _IRepositoryManger;
            IMapper = _IMapper;
            UserManager = _UserManager;
            IAuthenticationManger = _IAuthenticationManger;
        }




        [HttpPost("Register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterationUserDTO RegisterationUserDTO)
        {
            ApplicationUser ApplicationUser = IMapper.Map<ApplicationUser>(RegisterationUserDTO);

            IdentityResult result = await UserManager.CreateAsync(ApplicationUser, RegisterationUserDTO.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            await UserManager.AddToRolesAsync(ApplicationUser, RegisterationUserDTO.RolesOfTheUser);
            return StatusCode(201);

        }


        /*
        {
            "userName": "johndoe",
            "password": "mySecurePassword123"
        }
         */
        [HttpPost("Login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AuthinticateLogin([FromBody] AuthenticationUserDTO AuthenticationUserDTO)
        {
            if (!await IAuthenticationManger.AuthenticateUserLogin(AuthenticationUserDTO))
            {
                ILoggerManger.LogError("Authintication failed wrong username or password");
                return Unauthorized();
            }

            //1-create the roken
            string token = await IAuthenticationManger.CreateJWTToken();

            //2-create the cookie
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7), // Set an appropriate expiration date
                Path = "/", // Specify the cookie path
                HttpOnly = true, // Ensure the cookie is accessible only through HTTP
                Secure = true, // Ensure the cookie is only sent over HTTPS
                SameSite = SameSiteMode.None
            };

            // Set the cookie in the response heaser as Set-Cookie header
            Response.Cookies.Append("refresh_token", token, cookieOptions);
            return Ok(new { Token = token });//here we provide the user by access token just and not send to him refresh token


            // // Login request from frontend
            // fetch("https://localhost:7131/api/Authentication/login", {
            //   method: "POST",
            //   headers: { "Content-Type": "application/json" },

            //   //with (*) and (**) ,we send the tell the server to create the cookie in the broweser
            //   withCredentials: true, //(*)
            //   credentials: "include",  //(**)

            //   body: JSON.stringify({
            //     userName: "johndoe",
            //     password: "mySecurePassword123",
            //   }),
            // })
            //   .then((response) => response.json())
            //   .then((data) => {
            //      console.log(data);
            //   })
            //   .catch((error) => {
            //      console.error(error);
            //   });
        }














        [HttpGet("refreshToken")]

        public async Task<IActionResult> refreshToken()
        {
            //1-here we try to extract the header that named as cookie
            if (Request.Headers.TryGetValue("Cookie", out StringValues cookieHeaderValue))
            {
                //1-extract the cookie
                var cookies = cookieHeaderValue.ToString().Split(';');
                var myAccess_token = "";
                foreach (var cookie in cookies)
                {
                    var cookieParts = cookie.Split('=');
                    var cookieName = cookieParts[0].Trim();
                    var cookieValue = cookieParts[1].Trim();

                    if (cookieName == "refresh_token")
                    {
                        myAccess_token = cookieValue;
                        // Use the cookie value as needed
                        Console.WriteLine($"refresh_token is: {cookieValue}");
                        break;
                    }

                }

                //2-here we must validate the token we get
                //....


                //3-then we generate new acces token and resend it
                //but for now we just send an access token bc we not implment the refresh token
                string token = await IAuthenticationManger.CreateJWTToken();

                return Ok(new { Token = token });
                //return Ok();

            }

            else { return Unauthorized(); }


            // // refreshToken request from frontend
            // fetch("https://localhost:7131/api/Authentication/login", {
            //   method: "POST",
            //   headers: { "Content-Type": "application/json" },

            //   //with (*) and (**) ,we send the cookie as request header to the server 
            //   //then server analyse the cookie request header end extract the token
            //   // so without the 2 lines the request will not has the cookie request header
            //   withCredentials: true, //(*)
            //   credentials: "include",  //(**)

            //   body: JSON.stringify({
            //     userName: "johndoe",
            //     password: "mySecurePassword123",
            //   }),
            // })
            //   .then((response) => response.json())
            //   .then((data) => {
            //      console.log(data);
            //   })
            //   .catch((error) => {
            //      console.error(error);
            //   });


        }
    }
}
