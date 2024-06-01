using IMS.Models.Entities;
using Model.ViewModels.AccountModel;

namespace IMS_View.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountLoginModel> CheckLogin(string email, string password);
        Task<bool> SignUp(AccountRegisterModel accountRegisterModel);
    }
}
