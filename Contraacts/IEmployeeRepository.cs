using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contraacts
{
    public interface IEmployeeRepository
    {
        //CREATE
        void POSTEmployeeForCompany(Guid companyId, Employee Employee);

        //READ
        Task<PagedList<Employee>>  GetEmployeesAsync(Guid companyId, EmployeeParameters EmployeeParameters, bool trackChanges);
        Task<Employee> GetEmployeeByIdAsync(Guid companyId, Guid EmployeeId, bool trackChanges );

        //UPDATE

        //DELETE
        void DeleteEmployee(Employee employee);


    }
}
