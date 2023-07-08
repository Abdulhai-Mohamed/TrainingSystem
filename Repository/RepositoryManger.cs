using Contraacts;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    //RepositoryManger contain all  repositorries as properties, also we inject them by DI

    public class RepositoryManger : IRepositoryManger
    {
        private readonly RepositoryContext _RepositoryContext;
        private ICompanyRepository CompanyRepository;
        private IEmployeeRepository EmployeeRepository;

        public RepositoryManger(RepositoryContext RepositoryContext, ICompanyRepository _CompanyRepository, IEmployeeRepository _EmployeeRepository)
        {
            _RepositoryContext = RepositoryContext;
            CompanyRepository = _CompanyRepository;
            EmployeeRepository = _EmployeeRepository;

        }
        //instaed of manually(by new) create instance of CompanyRepository  to represent ICompanyRepository=>
        //i registerd CompanyRepository as ICompanyRepository  service
        public ICompanyRepository Company
        {

            get
            {
                //if (CompanyRepository == null)
                //{
                //    CompanyRepository = new CompanyRepository(_RepositoryContext);
                //}
                return CompanyRepository;
            }

        }

        public IEmployeeRepository Employee
        {

            get
            {
                //if (EmployeeRepository == null)
                //{
                //    EmployeeRepository = new EmployeeRepository(_RepositoryContext);
                //}
                return EmployeeRepository;
            }

        }

        public async Task SaveAsync()=>
          await  _RepositoryContext.SaveChangesAsync();
        
    }
}
