using Entities.Models;

namespace Contraacts
{
    public interface ICompanyRepository
    {
        //CREATE
        void POSTCompany(Company company);

        //READ
        Task<IEnumerable<Company>> GetAllCompinesAsync(bool trackChanges);
        Task<Company> GetCompanyByIdAsync(Guid CompanyId, bool trackChanges);

        Task<IEnumerable<Company>> GetAllCompinesByIdsAsync(IEnumerable<Guid> CompaniesIds, bool trackChanges);

        //UPDATE


        //DELETE
        void DeleteCompany(Company company);

    }
}
