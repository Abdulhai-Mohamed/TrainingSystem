using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contraacts
{
    public interface ICompanyRepository
    {
        //CREATE
        void POSTCompany(Company company);
       
        //READ
        Task<IEnumerable<Company>> GetAllCompinesAsync(bool trackChanges);
        Task< Company> GetCompanyByIdAsync(Guid CompanyId, bool trackChanges);

        Task<IEnumerable<Company>> GetAllCompinesByIdsAsync(IEnumerable<Guid> CompaniesIds, bool trackChanges);

        //UPDATE


        //DELETE
        void DeleteCompany(Company company);

    }
}
