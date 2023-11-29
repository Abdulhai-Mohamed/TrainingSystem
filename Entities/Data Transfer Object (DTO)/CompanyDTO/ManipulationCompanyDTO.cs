using Entities.Data_Transfer_Object__DTO_.EmployeeDTO;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Data_Transfer_Object__DTO_.CompanyDTO
{
    public class ManipulationCompanyDTO
    {

        [Required(ErrorMessage = "Company name is required field")]
        [MaxLength(70, ErrorMessage = "Maximum length for the name is 70 character")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Company adress is required field")]
        [MaxLength(60, ErrorMessage = "Maximum length for the adress is 60 character")]
        public string Address { get; set; }


        public string Country { get; set; }


        public IEnumerable<Employee> Employees { get; set; }
    }
}
