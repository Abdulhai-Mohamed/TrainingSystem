using Contraacts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        //private readonly IRepositoryManger IRepositoryManger;
        private readonly RepositoryContext RepositoryContext;

        public EmployeeRepository(RepositoryContext _RepositoryContext ) : base(_RepositoryContext)
        {
            RepositoryContext = _RepositoryContext;
        }

        //CREATE
        public void POSTEmployeeForCompany(Guid companyId, Employee Employee)
        {
            Company company = RepositoryContext.Set<Company>().Where(company=>company.Id.Equals(companyId)).SingleOrDefault();
            //Company company = IRepositoryManger.Company.GetCompanyById(companyId, trackChanges: false);
            Employee.CompanyOfTheEmployee = company;
            Employee.CompanyOfTheEmployee.Id= companyId;
            Create(Employee);
        }
        
        //READ
        public async Task<PagedList <Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters EmployeeParameters, bool trackChanges)
        //   =>
        //await   FindByCondition(employee=> employee.CompanyOfTheEmployee.Id.Equals(companyId), trackChanges: false)
        //   .OrderBy(emp=>emp.Name)
        //   .Skip( (EmployeeParameters.pageNumber - 1 )  *  EmployeeParameters.pageSize)//if page number 3 and size is 10=> we skip 3-1  * 10 == skip first 20 items  
        //   .Take( EmployeeParameters.pageSize)//after scip we take only page size items
        //   .ToListAsync();
        
        {

         List<Employee> ListEmployee =  
                await FindByCondition(employee => employee.CompanyOfTheEmployee.Id
               .Equals(companyId) /*&& (employee.Age>= EmployeeParameters.MinAge)&&(employee.Age <= EmployeeParameters.MaxAge)*/, trackChanges)
               .FilterEmployeeByAge(EmployeeParameters.MinAge, EmployeeParameters.MaxAge  )
               .SearchEmployee(EmployeeParameters.SearchTerm)
               //.OrderBy(emp => emp.Name)
               .SortEmployee(EmployeeParameters.orderBy)
               .ToListAsync();

            return PagedList<Employee>.ToPagedList(ListEmployee, EmployeeParameters.pageNumber, EmployeeParameters.pageSize) ;
        }


        public async Task<Employee> GetEmployeeByIdAsync(Guid companyId, Guid EmployeeId, bool trackChanges) =>
                      await FindByCondition(employee => 
                        employee.Id.Equals(EmployeeId) && employee.CompanyOfTheEmployee.Id.Equals(companyId),trackChanges).SingleOrDefaultAsync();
        //UPDATE

        //DELETE
        public void DeleteEmployee(Employee employee)=>
            Delete(employee);
    }
}
