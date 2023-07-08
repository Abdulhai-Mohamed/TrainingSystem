using Contraacts;
using Entities.Data_Transfer_Object__DTO_.ApplicationUserDTO;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TrainingSystem.Utility
{
    public class AuthenticationManger : IAuthenticationManger
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IConfiguration IConfiguration;
        private ApplicationUser ApplicationUser;


        public AuthenticationManger(UserManager<ApplicationUser> _UserManager, IConfiguration _IConfiguration)
        {
            UserManager = _UserManager;
            IConfiguration = _IConfiguration;

        }

        public async Task<bool> AuthenticateUserLogin(AuthenticationUserDTO AuthenticationUserDTO)
        {
            ApplicationUser = await UserManager.FindByNameAsync(AuthenticationUserDTO.UserName);

            return (ApplicationUser != null && await UserManager.CheckPasswordAsync(ApplicationUser, AuthenticationUserDTO.Password));

        }

        public async Task<string> CreateJWTToken()
        {
            SigningCredentials signingCredentials = GetSigningCredentials();
            List<Claim> claims = await GetClaimsAsync();
            JwtSecurityToken JwtSecurityToken = GetJwtSecurityToken(signingCredentials, claims);


            return new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken);
            //JWT:
            //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiam9obmRvZSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ik1hbmFnZXIiLCJleHAiOjE2ODAwNDk5MDcsImlzcyI6IkFiZG9BcGkiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MTMxIn0.R62X-nISIOetyl7eSbxZfxa7Mu3OffHS1vnixGNUt98
            //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiam9obmRvZSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ik1hbmFnZXIiLCJleHAiOjE2ODAxNTM3OTUsImlzcyI6IkFiZG9BcGkiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MTMxIn0.o2hS9zfR4pl0Ad41hSouiyDn4NMXuNTezcRXXbKdnfY
            //to decode it look at jwt.io site
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
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: SigningCredentials
                );

            return JwtSecurityToken;
        }

    }
}
