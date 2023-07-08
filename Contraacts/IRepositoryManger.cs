using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contraacts
{
    public interface IRepositoryManger
    {
        ICompanyRepository Company  { get; }
        IEmployeeRepository Employee  { get; }
        Task SaveAsync();
    }
}
