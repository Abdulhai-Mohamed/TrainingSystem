using Contraacts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext RepositoryContext) : base(RepositoryContext)
        {
        }

        //CREATE
        public void POSTCompany(Company company) =>
            Create(company);
        
        //READ
        public async Task<IEnumerable<Company>> GetAllCompinesAsync(bool trackChanges)=>
           await FindAll(trackChanges).OrderBy(company => company.Name).ToListAsync();

        public async Task<Company> GetCompanyByIdAsync(Guid CompanyId, bool trackChanges)=>
           await  FindByCondition(company => company.Id.Equals(CompanyId), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Company>> GetAllCompinesByIdsAsync(IEnumerable<Guid> CompaniesIds, bool trackChanges)=>
                     await FindByCondition(company => CompaniesIds.Contains(company.Id), trackChanges).ToListAsync();


        //UPDATE

        //DELETE
        public void DeleteCompany(Company company)=>
            Delete(company);

    }
}
