using IMS.Models.Entities;
using IMS.Models.ViewModels;

namespace IMS_View.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Account> CheckLogin(string email, string password);
        Task<bool> SignUp(AccountRegisterModel accountRegisterModel);
    }
}
