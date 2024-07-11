using IMS.Repositories.AccountModel;
using IMS.Repositories.Entities;

namespace IMS.Repositories.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account> GetAccountByMail(string email);
        Task<List<AccountGetModel>> GetAllAccountsWithRole(int pageSize, int pageNumber, string searchTerm);
        IQueryable<Account> GetAll();
        Task<List<Account>> GetMentorAccount();
    }
}
