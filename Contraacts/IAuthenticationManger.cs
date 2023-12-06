using Entities.Data_Transfer_Object__DTO_.ApplicationUserDTO;
using Entities.Models;

namespace Contraacts
{
    public interface IAuthenticationManger
    {

        Task<bool> AuthenticateUserLogin(PostLoginUserDTO PostLoginUserDTO);
        Task<GETAuthenticateResponseDTO> LoginProcess(ApplicationUser ApplicationUser, string message, bool refreshTokenRotation = false);

        Task<GETAuthenticateResponseDTO> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);

        Task<object> CreateJWTToken();
        RefreshToken CreateRefreshToken();

        Task<string> AddRoleAsync(POSTCreateRole POSTCreateRole);


    }
}
