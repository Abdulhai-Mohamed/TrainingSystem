using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Data_Transfer_Object__DTO_.EmployeeDTO
{
    public class GETEmployeeDTO : ManipulationEmployeeDTO
    {
        public Guid Id { get; set; }

    }
}
