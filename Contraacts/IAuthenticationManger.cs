using Entities.Data_Transfer_Object__DTO_.ApplicationUserDTO;

namespace Contraacts
{
    public interface IAuthenticationManger
    {

        Task<bool> AuthenticateUserLogin(AuthenticationUserDTO AuthenticationUserDTO);
        Task<string> CreateJWTToken();
    }
}
