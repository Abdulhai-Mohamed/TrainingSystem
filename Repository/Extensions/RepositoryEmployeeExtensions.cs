using Entities.Models;
using Entities.RequestFeatures;
using Repository.Extensions.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> FilterEmployeeByAge(this IQueryable<Employee> EmployeeQuery, uint minAge, uint maxAge) =>
                   EmployeeQuery.Where(employee => (employee.Age >= minAge) && (employee.Age <= maxAge));



        public static IQueryable<Employee> SearchEmployee(this IQueryable<Employee> EmployeeQuery ,string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return EmployeeQuery;

            string LowerCaseTerm = searchTerm.Trim().ToLower();
            return EmployeeQuery.Where(Employee => Employee.Name.ToLower().Contains(LowerCaseTerm));
        }


        public static IQueryable<Employee> SortEmployee(this IQueryable<Employee> EmployeeQuery, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString)) EmployeeQuery.OrderBy(emp => emp.Name); //default order

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery)) EmployeeQuery.OrderBy(emp => emp.Name); //default order

            return EmployeeQuery.OrderBy(orderQuery);
        }


    }
}
