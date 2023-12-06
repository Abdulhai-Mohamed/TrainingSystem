using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Employee
    {
        [Column("EmployeeId")]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "Employee name is required field")]
        [MaxLength(30, ErrorMessage = "Maximum length for the name is 30 character")]
        public string Name { get; set; }


        [Required(ErrorMessage = "age  is required field")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Position  is required field")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 character")]
        public string Position { get; set; }


        //this column will represent the forign ke column in which reference to company table
        //we will use this columns to store the id of employe's company
        //its name in db set by efcore to CompanyOfTheEmployeeId
        public Company CompanyOfTheEmployee { get; set; }
    }
}
