namespace Contraacts
{
    public interface IRepositoryManger
    {
        ICompanyRepository Company { get; }
        IEmployeeRepository Employee { get; }
        Task SaveAsync();
    }
}
