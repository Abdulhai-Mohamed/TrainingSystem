using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Data_Transfer_Object__DTO_.ApplicationUserDTO
{
    public class PostRegisterationUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "User Name required field")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required field")] 
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        
        public ICollection<string> RolesOfTheUser { get; set; }
            

    }
}
