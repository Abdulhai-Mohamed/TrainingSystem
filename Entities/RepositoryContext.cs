using Entities.Configurations;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class RepositoryContext : /*DbContext*/ IdentityDbContext<ApplicationUser>
    {
        public RepositoryContext(DbContextOptions options):base(options) 
        {
            
        }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //sedding
            //internal:
            //modelBuilder.Entity<Employee>().HasData(
            //                  new Employee
            //                  {
            //                      Id = new Guid("67adda78-4608-42e7-9463-3773e0ffa4db"),
            //                      Name = "Ahmed",
            //                      Age = 30,
            //                      Position = "SE Engineer",
            //                  });

            //External  
          modelBuilder.ApplyConfiguration(new CompanyConfiguration());
          modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
          modelBuilder.ApplyConfiguration(new RoleConfiguration());



            // // ALTER TABLE [Employees] ADD CONSTRAINT [FK_Employees_Companies_CompanyOfTheEmployeeId] FOREIGN KEY ([CompanyOfTheEmployeeId]) REFERENCES [Companies] ([ComapnyId]) ON DELETE NO ACTION;
            //default=>
            //      ALTER TABLE [Employees] ADD CONSTRAINT [FK_Employees_Companies_CompanyOfTheEmployeeId] FOREIGN KEY ([CompanyOfTheEmployeeId]) REFERENCES [Companies] ([ComapnyId]) ON DELETE CASCADE;

            IEnumerable<IMutableForeignKey> ForeignKeys =
           modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());

            foreach (IMutableForeignKey FK in ForeignKeys)
            {
                FK.DeleteBehavior = DeleteBehavior.Cascade;
            }




            base.OnModelCreating(modelBuilder);

        }

    }

}
