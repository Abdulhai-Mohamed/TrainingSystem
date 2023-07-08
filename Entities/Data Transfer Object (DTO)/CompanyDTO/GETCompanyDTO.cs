using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Data_Transfer_Object__DTO_.CompanyDTO
{
    public class GETCompanyDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FullAdress { get; set; }
    }
}
