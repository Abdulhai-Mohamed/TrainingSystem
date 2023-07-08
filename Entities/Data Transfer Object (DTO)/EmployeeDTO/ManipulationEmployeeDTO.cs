using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Data_Transfer_Object__DTO_.EmployeeDTO
{
    public class ManipulationEmployeeDTO
    {




        [Required(ErrorMessage = "Employee name is required field")]
        [MaxLength(30, ErrorMessage = "Maximum length for the name is 30 character")]
        public string Name { get; set; }


        [Required(ErrorMessage = "age  is required field")]
        [Range(18, int.MaxValue, ErrorMessage = "age  is required field and it can't be lower than 18")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Position  is required field")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 character")]
        public string Position { get; set; }

    }
}
