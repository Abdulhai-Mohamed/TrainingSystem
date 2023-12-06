using Contraacts;
using Entities.Data_Transfer_Object__DTO_.ApplicationUserDTO;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace TrainingSystem.Utility
{
    public class AuthenticationManger : IAuthenticationManger
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IConfiguration IConfiguration;
        private ApplicationUser ApplicationUser;
        private readonly RoleManager<IdentityRole> roleManager;



        public AuthenticationManger(UserManager<ApplicationUser> _UserManager, IConfiguration _IConfiguration, RoleManager<IdentityRole> _roleManager)
        {
            UserManager = _UserManager;
            IConfiguration = _IConfiguration;
            roleManager = _roleManager;
        }

        public async Task<bool> AuthenticateUserLogin(PostLoginUserDTO PostLoginUserDTO)
        {
            ApplicationUser = await UserManager.FindByNameAsync(PostLoginUserDTO.UserName);

            return (ApplicationUser != null && await UserManager.CheckPasswordAsync(ApplicationUser, PostLoginUserDTO.Password));

        }

        public async Task<GETAuthenticateResponseDTO> LoginProcess(ApplicationUser ApplicationUser, string message, bool refreshTokenRotation = false)
        {
            //1-get roles of the user
            IList<string> Theroles = await UserManager.GetRolesAsync(ApplicationUser);
            var roles = Theroles.ToList();

            //2-create jwt access token and access the token prop by reflection, bc we can't access it directly bc the obj is anonymous
            object JWTObj = await CreateJWTToken();
            string JWTToken = (string)JWTObj?.GetType().GetProperty("token")?.GetValue(JWTObj);
            DateTime JWTTokenExpire = (DateTime)JWTObj?.GetType().GetProperty("JWTExpire")?.GetValue(JWTObj);

            //3-GETAuthenticateResponseDTO
            GETAuthenticateResponseDTO GETAuthenticateResponseDTO = new GETAuthenticateResponseDTO
            {
                Username = ApplicationUser.UserName,
                Email = ApplicationUser.Email,
                Message = message,
                Roles = roles,
                IsAuthenticated = true,
                AccessToken = JWTToken,
                ExpiresOn = JWTTokenExpire,
            };

            //4-create refresh token and add it to the refresh tokens user's table
            RefreshToken refreshToken;
            if (refreshTokenRotation)
            {
                refreshToken = CreateRefreshToken();
                GETAuthenticateResponseDTO.RefreshToken = refreshToken.Token;
                GETAuthenticateResponseDTO.RefreshTokenExpiration = refreshToken.ExpiresOn;
                ApplicationUser.RefreshTokens.Add(refreshToken);
                await UserManager.UpdateAsync(ApplicationUser);
            }
            if (ApplicationUser.RefreshTokens.Any(t => t.IsActive))
            {
                refreshToken = ApplicationUser.RefreshTokens.FirstOrDefault(t => t.IsActive);
                GETAuthenticateResponseDTO.RefreshToken = refreshToken.Token;
                GETAuthenticateResponseDTO.RefreshTokenExpiration = refreshToken.ExpiresOn;
            }
            else
            {
                refreshToken = CreateRefreshToken();
                GETAuthenticateResponseDTO.RefreshToken = refreshToken.Token;
                GETAuthenticateResponseDTO.RefreshTokenExpiration = refreshToken.ExpiresOn;
                ApplicationUser.RefreshTokens.Add(refreshToken);
                await UserManager.UpdateAsync(ApplicationUser);
            }






            //5-send and set the refresh token in cookie
            //set by controller

            return GETAuthenticateResponseDTO;
        }



        public async Task<GETAuthenticateResponseDTO> RefreshTokenAsync(string token)
        {
            GETAuthenticateResponseDTO GETAuthenticateResponseDTO = new GETAuthenticateResponseDTO();

            var user = await UserManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                GETAuthenticateResponseDTO.Message = "Invalid token";
                return GETAuthenticateResponseDTO;
            }


            RefreshToken refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                GETAuthenticateResponseDTO.Message = "Inactive token";
                return GETAuthenticateResponseDTO;
            }
            if (refreshToken.IsRevoked)
            {
                user.RefreshTokens = null;//kill al family 
                await UserManager.UpdateAsync(user);
                GETAuthenticateResponseDTO.Message = "this refresh token used before";
                GETAuthenticateResponseDTO.IsAuthenticated = false;

                return GETAuthenticateResponseDTO;
            }

            refreshToken.RevokedOn = DateTime.UtcNow;




            GETAuthenticateResponseDTO = await LoginProcess(user, "New Refresh Token and Access token are returned Successfully", true);

            return (GETAuthenticateResponseDTO);



        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await UserManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            RefreshToken refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;

            await UserManager.UpdateAsync(user);

            return true;
        }


        public async Task<object> CreateJWTToken()
        {
            SigningCredentials signingCredentials = GetSigningCredentials();
            List<Claim> claims = await GetClaimsAsync();
            JwtSecurityToken JwtSecurityToken = GetJwtSecurityToken(signingCredentials, claims);


            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken),
                JWTExpire = JwtSecurityToken.ValidTo
            };
            //JWT:
            //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiam9obmRvZSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ik1hbmFnZXIiLCJleHAiOjE2ODAwNDk5MDcsImlzcyI6IkFiZG9BcGkiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MTMxIn0.R62X-nISIOetyl7eSbxZfxa7Mu3OffHS1vnixGNUt98
            //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiam9obmRvZSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ik1hbmFnZXIiLCJleHAiOjE2ODAxNTM3OTUsImlzcyI6IkFiZG9BcGkiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MTMxIn0.o2hS9zfR4pl0Ad41hSouiyDn4NMXuNTezcRXXbKdnfY
            //to decode it look at jwt.io site
        }

        public RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(10)
            };
        }



        public async Task<string> AddRoleAsync(POSTCreateRole POSTCreateRole)
        {
            ApplicationUser ApplicationUser = await UserManager.FindByIdAsync(POSTCreateRole.UserId);

            if (ApplicationUser is null || !await roleManager.RoleExistsAsync(POSTCreateRole.Role))
                return "Invalid user ID or Role";

            if (await UserManager.IsInRoleAsync(ApplicationUser, POSTCreateRole.Role))
                return "User already assigned to this role";

            var result = await UserManager.AddToRoleAsync(ApplicationUser, POSTCreateRole.Role);

            return result.Succeeded ? string.Empty : "Sonething went wrong";
        }






        private SigningCredentials GetSigningCredentials()
        {
            //1: Read secret key from env,  encode and has key as array of bytes
            byte[] key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));


            //2-
            //SymmetricSecurityKey object represents a symmetric security key that can be used for signing
            //and/or encrypting security tokens. The key value is passed as an argument to the constructor,
            //and the resulting instance is stored in the secret variable.
            SymmetricSecurityKey secret = new SymmetricSecurityKey(key);


            //3-
            //SigningCredentials object  used to sign a JSON Web Token (JWT) with a secret key.
            //The secret instance created in step 2 is used as the key to sign the token,
            //and the SecurityAlgorithms.HmacSha256 constant is used as the algorithm for signing the token. 
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        }

        private async Task<List<Claim>> GetClaimsAsync()
        {
            if (ApplicationUser == null)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,"refresh token claim")
                };
                return claims;
            }
            else
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,ApplicationUser.UserName)//{hhttp://schemas.xmlsoap.org/ws/2005/05/identity/claims/name: johndoe}
                };


                IList<string> roles = await UserManager.GetRolesAsync(ApplicationUser);

                foreach (string role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));//{hhttp://schemas.microsoft.com/ws/2008/06/identity/claims/role: Manager}
                }

                return claims;

            }




        }


        private JwtSecurityToken GetJwtSecurityToken(SigningCredentials SigningCredentials, List<Claim> claims)
        {
            IConfiguration JWtSettings = IConfiguration.GetSection("JWtSettings");

            JwtSecurityToken JwtSecurityToken = new JwtSecurityToken
                (
                issuer: JWtSettings.GetSection("validIssuer").Value,
                audience: JWtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: SigningCredentials
                );

            return JwtSecurityToken;
        }


    }
}
