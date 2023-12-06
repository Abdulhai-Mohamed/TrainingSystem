using System.ComponentModel.DataAnnotations;

namespace Entities.Data_Transfer_Object__DTO_.ApplicationUserDTO
{
    public class PostLoginUserDTO
    {
        [Required(ErrorMessage = "User Name is required field")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required field")]
        public string Password { get; set; }
    }
}
