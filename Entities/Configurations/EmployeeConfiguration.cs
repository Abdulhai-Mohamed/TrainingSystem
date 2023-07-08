using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configurations
{
   
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> EntityTypeBuilder)
        {
            EntityTypeBuilder.HasData(
                                new Employee
                                {
                                    Id = new Guid("67adda78-4608-42e7-9463-3773e0ffa4db"),
                                    Name = "Ahmed",
                                    Age = 30,
                                    Position = "SE Engineer"
                                },
                                new Employee
                                {
                                    Id = new Guid("3e0f6702-f265-4e61-912a-66140071974c"),
                                    Name = "John",
                                    Age = 40,
                                    Position = "DEVOPS"
                                },
                                new Employee
                                {
                                    Id = new Guid("5134eb68-3a7e-4042-a549-b92b87abbc03"),
                                    Name = "Mai",
                                    Age = 50,
                                    Position = "Tester"
                                },
                                new Employee
                                {
                                    Id = new Guid("7c3ea679-55e9-42fe-908d-65b5823f54c0"),
                                    Name = "Sara",
                                    Age = 20,
                                    Position = "Product Owner"
                                }
                            );
        }
    }


}
