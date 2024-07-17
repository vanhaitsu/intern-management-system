using AutoMapper;
using IMS.Models.Interfaces;
using IMS.Repositories.AccountModel;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using IMS.Repositories.Models.AccountModel;
using IMS.Repositories.Models.CommonModel;
using IMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

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

        public async Task<LoginResult> CheckLogin(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new LoginResult { ErrorMessage = "Email is required." };
            }
            var account = await _unitOfWork.AccountRepository.GetAccountByMail(email);
            if (account == null || !password.Equals(account.Password))
            {
                return new LoginResult { ErrorMessage = "Email or password is not correct." };
            }
            if (account.IsDeleted)
            {
                return new LoginResult { ErrorMessage = "Your account was blocked." };
            }
            var role = await _unitOfWork.RoleRepository.GetAsync(account.RoleId.Value);
            var loginModel = new LoginModel
            {
                Id = account.Id,
                Email = account.Email,
                Password = account.Password,
                FullName = account.FullName,
                Role = role.Name
            };

            return new LoginResult { LoginModel = loginModel };
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
                existedAccount.Id = id;
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

        public async Task<List<AccountGetModel>> GetAllAccounts(AccountFilterModel filterModel)
        {
            var accountList = await _unitOfWork.AccountRepository.GetAllAsync(
                filter: x =>
                    (filterModel.Role == null || x.Role.Name == filterModel.Role) &&
                    (string.IsNullOrEmpty(filterModel.Search) || x.FullName.ToLower().Contains(filterModel.Search.ToLower()) ||
                     x.Email.ToLower().Contains(filterModel.Search.ToLower())),
                orderBy: x => filterModel.OrderByDescending
                    ? x.OrderByDescending(x => x.CreationDate)
                    : x.OrderBy(x => x.CreationDate),
                pageIndex: filterModel.PageNumber,
                pageSize: filterModel.PageSize,
                includeProperties: "Role"
            );

            List<AccountGetModel> accountDetailList = null;

            if (accountList != null)
            {
                accountDetailList = accountList.Data.Select(x => new AccountGetModel
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email,
                    DOB = x.DOB,
                    Gender = x.Gender,
                    PhoneNumber = x.PhoneNumber,
                    Address = x.Address,
                    Status = x.IsDeleted,
                    RoleId = x.RoleId,
                    RoleName = x.Role?.Name 
                }).ToList();
            }

            return accountDetailList ?? new List<AccountGetModel>();
        }


        public async Task<int> GetTotalAccountsCount(AccountFilterModel filterModel)
        {
            IQueryable<Account> query = _unitOfWork.AccountRepository.GetAll().AsQueryable();


            if (!string.IsNullOrEmpty(filterModel.Search))
            {
                var search = filterModel.Search.ToLower();
                query = query.Where(a =>
                    a.FullName.ToLower().Contains(search) ||
                    a.Email.ToLower().Contains(search)
                );
            }
            if (!string.IsNullOrEmpty(filterModel.Role))
            {
                query = query.Where(a => a.Role.Name == filterModel.Role);
            }

            return await query.CountAsync();
        }

        public async Task<List<Account>> GetMentorAccount()
        {
            var listRoleMentor = await _unitOfWork.RoleRepository.GetMentorsRole();
            List<Account> accounts = new List<Account>();
            foreach (var role in listRoleMentor)
            {
                if (role.Accounts != null)
                {
                    accounts.AddRange(role.Accounts);
                }
            }

            return accounts;
        }
    }
}
