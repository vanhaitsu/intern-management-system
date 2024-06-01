using IMS.Models.Entities;

namespace IMS.Models.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account> GetAccountByMail(string email);
    }
}
