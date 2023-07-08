using Entities.Data_Transfer_Object__DTO_.ApplicationUserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contraacts
{
     public interface IAuthenticationManger
    {

        Task<bool> AuthenticateUserLogin(AuthenticationUserDTO AuthenticationUserDTO);
        Task<string> CreateJWTToken();
    }
}
