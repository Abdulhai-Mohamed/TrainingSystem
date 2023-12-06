using AutoMapper;
using Contraacts;
using Entities.Data_Transfer_Object__DTO_.ApplicationUserDTO;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> RegisterUser([FromBody] PostRegisterationUserDTO PostRegisterationUserDTO)
        {



            //1-
            //Manual Filtering
            //if (PostRegisterationUserDTO == null)
            //{
            //    ILoggerManger.LogError($"PostRegisterationUserDTO object set from client is null.");
            //    return BadRequest("PostRegisterationUserDTO Is null.");
            //}
            //if (!ModelState.IsValid)
            //{
            //    ILoggerManger.LogError($"Invalid ModelState For the PostRegisterationUserDTO object ");
            //    return UnprocessableEntity(ModelState);
            //}
            //2-
            //map from PostRegisterationUserDTO to ApplicationUser
            ApplicationUser ApplicationUser = IMapper.Map<ApplicationUser>(PostRegisterationUserDTO);

            IdentityResult result = await UserManager.CreateAsync(ApplicationUser, PostRegisterationUserDTO.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            await UserManager.AddToRolesAsync(ApplicationUser, PostRegisterationUserDTO.RolesOfTheUser);

            GETAuthenticateResponseDTO GETAuthenticateResponseDTO = await IAuthenticationManger.LoginProcess(ApplicationUser, "User Created and logged-in Successfully");

            SetRefreshTokenInCookie(GETAuthenticateResponseDTO.RefreshToken, GETAuthenticateResponseDTO.RefreshTokenExpiration);

            return Ok(GETAuthenticateResponseDTO);


        }


        /*
        {
            "userName": "johndoe",
            "password": "mySecurePassword123"
        }
         */
        [HttpPost("Login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AuthinticateLogin([FromBody] PostLoginUserDTO PostLoginUserDTO)
        {
            if (!await IAuthenticationManger.AuthenticateUserLogin(PostLoginUserDTO))
            {
                ILoggerManger.LogError("Authintication failed wrong username or password");
                return Unauthorized();
            }
            var applicationUser = await UserManager.FindByNameAsync(PostLoginUserDTO.UserName);


            GETAuthenticateResponseDTO GETAuthenticateResponseDTO = await IAuthenticationManger.LoginProcess(applicationUser, "User Logged in Successfully");

            SetRefreshTokenInCookie(GETAuthenticateResponseDTO.RefreshToken, GETAuthenticateResponseDTO.RefreshTokenExpiration);

            return Ok(GETAuthenticateResponseDTO);




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
        public async Task<IActionResult> RefreshToken()
        {
            string refreshTokenCookie = Request.Cookies["refreshTokenCookie"];

            GETAuthenticateResponseDTO GETAuthenticateResponseDTO = await IAuthenticationManger.RefreshTokenAsync(refreshTokenCookie);

            if (!GETAuthenticateResponseDTO.IsAuthenticated)
                return BadRequest(GETAuthenticateResponseDTO);

            SetRefreshTokenInCookie(GETAuthenticateResponseDTO.RefreshToken, GETAuthenticateResponseDTO.RefreshTokenExpiration);

            return Ok(GETAuthenticateResponseDTO);
            //    // // refreshToken request from frontend
            //    // fetch("https://localhost:7131/api/Authentication/login", {
            //    //   method: "POST",
            //    //   headers: { "Content-Type": "application/json" },

            //    //   //with (*) and (**) ,we send the cookie as request header to the server 
            //    //   //then server analyse the cookie request header end extract the token
            //    //   // so without the 2 lines the request will not has the cookie request header
            //    //   withCredentials: true, //(*)
            //    //   credentials: "include",  //(**)

            //    //   body: JSON.stringify({
            //    //     userName: "johndoe",
            //    //     password: "mySecurePassword123",
            //    //   }),
            //    // })
            //    //   .then((response) => response.json())
            //    //   .then((data) => {
            //    //      console.log(data);
            //    //   })
            //    //   .catch((error) => {
            //    //      console.error(error);
            //    //   });
            //}


        }


        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] PostRevokeTokenDTO PostRevokeTokenDTO)
        {
            var token = PostRevokeTokenDTO.Token ?? Request.Cookies["refreshTokenCookie"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await IAuthenticationManger.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is invalid!");

            return Ok();
        }



        [HttpPost("addRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] POSTCreateRole POSTCreateRole)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await IAuthenticationManger.AddRoleAsync(POSTCreateRole);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(POSTCreateRole);
        }



        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshTokenCookie", refreshToken, cookieOptions);
        }



    }
}
