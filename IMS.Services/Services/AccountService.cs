using AutoMapper;
using IMS.Repositories.AccountModel;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using IMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Services.Services
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


        public async Task<bool> CheckExistedAccount(string email)
        {
            Account account = new Account();
            account = await _unitOfWork.AccountRepository.GetAccountByMail(email);
            if (account == null)
            {
                return false;
            }
            return true;
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

        public async Task<bool> Create(AccountRegisterModel accountRegisterModel)
        {
            Account user = _mapper.Map<Account>(accountRegisterModel);
            Role role = await _unitOfWork.RoleRepository.GetAsync(accountRegisterModel.RoleId);
            user.RoleId = role.Id;
            user.IsDeleted = false;
            user.Role = role;
            _unitOfWork.AccountRepository.AddAsync(user);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<Account> GetAccountAsync(Guid id)
        {
            var accounts = await _unitOfWork.AccountRepository.GetAsync(id);
            if (accounts != null)
            {
                return accounts;
            }
            return null;
        }

        public async Task<bool> Update(Guid id, AccountUpdateModel accountUpdateModel)
        {
            var existedAccount = await _unitOfWork.AccountRepository.GetAsync(id);
            if (existedAccount != null)
            {
                var roleExists = await _unitOfWork.RoleRepository.GetAsync(accountUpdateModel.RoleId);
                if (roleExists == null)
                {
                    return false;
                }
                _mapper.Map(accountUpdateModel, existedAccount);
                // existedAccount.RoleId = accountUpdateModel.RoleId;
                existedAccount.Role = roleExists;
                _unitOfWork.AccountRepository.Update(existedAccount);
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
            if (existedAccount != null)
            {
                _unitOfWork.AccountRepository.SoftDelete(existedAccount);
            }
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Restore(Guid id)
        {
            var existedAccount = await _unitOfWork.AccountRepository.GetAsync(id);
            if (existedAccount != null)
            {
                existedAccount.IsDeleted = false;
                _unitOfWork.AccountRepository.Update(existedAccount);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<List<AccountGetModel>> GetAllAccounts(int pageSize, int pageNumber, string searchTerm)
        {
            return await _unitOfWork.AccountRepository.GetAllAccountsWithRole(pageSize, pageNumber, searchTerm);
        }

        public async Task<int> GetTotalAccountsCount(string searchTerm)
        {
            IQueryable<Account> query = _unitOfWork.AccountRepository.GetAll().AsQueryable();


            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a =>
                    a.FullName.Contains(searchTerm.ToLower()) ||
                    a.Email.Contains(searchTerm.ToLower()) ||
                    a.PhoneNumber.Contains(searchTerm.ToLower())
                );
            }

            return await query.CountAsync();
        }
        public async Task<List<AccountGetModel>> SearchAccountsAsync(string searchTerm)
        {
            List<AccountGetModel> accountGetModels = new List<AccountGetModel>();
            //List<Account> accounts = await _unitOfWork.AccountRepository.GetAllAsync();
            //List<Role> roles = await _unitOfWork.RoleRepository.GetAllAsync();

            List<Account> accounts = null;
            List<Role> roles = null;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accounts = accounts.Where(a =>
                    a.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    a.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    a.PhoneNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            foreach (Account account in accounts)
            {
                var accountModel = _mapper.Map<AccountGetModel>(account);
                accountModel.Status = account.IsDeleted;
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
