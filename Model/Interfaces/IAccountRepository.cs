using IMS.Models.Entities;
using Model.ViewModels.AccountModel;

namespace IMS.Models.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account> GetAccountByMail(string email);
        Task<List<AccountGetModel>> GetAllAccountsWithRole(int pageSize, int pageNumber, string searchTerm);
        IQueryable<Account> GetAll();
    }
}
