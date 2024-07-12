using IMS.Repositories.AccountModel;
using IMS.Repositories.Entities;
using IMS.Repositories.Models.AccountModel;
using IMS.Repositories.Models.CommonModel;

namespace IMS.Services.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResult> CheckLogin(string email, string password);
        Task<bool> CheckExistedAccount(string email);
        Task<Account> GetAccountAsync(Guid id);
       // Task<bool> SignUp(AccountRegisterModel accountRegisterModel);
        Task<bool> Update(Guid id,AccountUpdateModel accountUpdateModel);
        Task<bool> Delete(Guid id);
        Task<List<AccountGetModel>> GetAllAccounts(AccountFilterModel filterModel);
        Task<bool> Create(AccountRegisterModel accountRegisterModel);
        Task<int> GetTotalAccountsCount(AccountFilterModel filterModel);
        Task<bool> Restore(Guid id);
        Task<List<Account>> GetMentorAccount();
    }
}
