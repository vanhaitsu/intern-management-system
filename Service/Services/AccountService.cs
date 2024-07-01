using AutoMapper;
using IMS.Models.Entities;
using IMS.Models.Interfaces;
using IMS_View.Services.Interfaces;
using Model.Enums;
using Model.ViewModels.AccountModel;

namespace IMS_View.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AccountLoginModel> CheckLogin(string email, string password)
        {
            Account account = new Account();
            AccountLoginModel loginModel = new AccountLoginModel();
            if (!string.IsNullOrEmpty(email))
            {
                account = await _unitOfWork.AccountRepository.GetAccountByMail(email);
                if (account == null || !password.Equals(account.Password))
                {
                    return null;
                }
                else
                {
                    Role role = await _unitOfWork.RoleRepository.GetAsync(account.RoleId.Value);
                    loginModel.Id = account.Id;
                    loginModel.Email = account.Email;
                    loginModel.Password = account.Password;
                    loginModel.FullName = account.FullName;
                    loginModel.PhoneNumber = account.PhoneNumber;
                    loginModel.DOB = account.DOB;
                    loginModel.Gender = account.Gender;
                    loginModel.Address = account.Address;
                    loginModel.Role = role.Name;
                }
            }
            return loginModel;
        }

        public async Task<bool> SignUp(AccountRegisterModel accountRegisterModel)
        {
            Account user = _mapper.Map<Account>(accountRegisterModel);
            Role role = await _unitOfWork.RoleRepository.GetByName("Admin");
            user.RoleId = role.Id;
            _unitOfWork.AccountRepository.AddAsync(user);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Account>> GetAll()
        {
            var accounts = await _unitOfWork.AccountRepository.GetAllAsync();
            if(accounts != null)
            {
                return accounts;
            }
            return null;
        }

        public async Task<bool> Update(Guid id, AccountUpdateModel accountUpdateModel)
        {
            var existedAccount = _unitOfWork.AccountRepository.GetAsync(id);
            if (existedAccount != null)
            {
                Role role = await _unitOfWork.RoleRepository.GetByName(accountUpdateModel.RoleName);
                Account user = _mapper.Map<Account>(accountUpdateModel);
                user.RoleId = role.Id;
                _unitOfWork.AccountRepository.Update(user);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var existedAccount = await _unitOfWork.AccountRepository.GetAsync(id);
            if(existedAccount != null)
            {
                _unitOfWork.AccountRepository.SoftDelete(existedAccount);
            }
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<AccountGetModel>> GetAllAccounts()
        {
            List<AccountGetModel> accountGetModels = new List<AccountGetModel>();
            List<Account> accounts = await _unitOfWork.AccountRepository.GetAllAsync();
            List<Role> roles = await _unitOfWork.RoleRepository.GetAllAsync();
            foreach (Account account in accounts)
            {
                var accountModel = _mapper.Map<AccountGetModel>(account);
                foreach (Role role in roles)
                {
                    if (account.RoleId == role.Id)
                    {
                        accountModel.RoleName = role.Name;
                    }
                }
                accountGetModels.Add(accountModel);
            }
            return accountGetModels;
        }
    }
}
