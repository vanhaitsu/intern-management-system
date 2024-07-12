using IMS.Models.Repositories;
using IMS.Repositories.AccountModel;
using IMS.Repositories.Entities;
using IMS.Repositories.Enums;
using IMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Repositories.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly AppDbContext _dbContext;

        public AccountRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account> GetAccountByMail(string email)
        {
            return await _dbContext.Accounts.SingleOrDefaultAsync(a => a.Email.Equals(email));
        }
        public IQueryable<Account> GetAll()
        {
            return _dbContext.Accounts.AsQueryable();
        }

        public async Task<List<AccountGetModel>> GetAllAccountsWithRole(int pageSize, int pageNumber, string searchTerm)
        {
            IQueryable<Account> query = _dbContext.Accounts.Include(a => a.Role);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a =>
                    a.FullName.Contains(searchTerm.ToLower()) ||
                    a.Email.Contains(searchTerm.ToLower()) ||
                    a.PhoneNumber.Contains(searchTerm.ToLower())
                );
            }

            var accountModels = await query
                .Select(a => new AccountGetModel
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Address = a.Address,
                    DOB = a.DOB,
                    Email = a.Email,
                    Gender = a.Gender,
                    PhoneNumber = a.PhoneNumber,
                    RoleId = a.RoleId,
                    RoleName = a.Role.Name,
                    Status = a.IsDeleted
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return accountModels;
        }

        public async Task<List<Account>> GetMentorAccount()
        {
            var Gasdasduid = Guid.Parse("9470D747-3A06-4939-9E58-A1166506FBB0");

 
            var mentorAccounts = await _dbContext.Accounts
                .Where(a => a.RoleId == Gasdasduid)
                .ToListAsync();
            return mentorAccounts;



        }

        public String getNamebyId(Guid? id)
        {
            var account =  _dbContext.Accounts.Find(id) ;
            if(account != null) return account.FullName;
            return null;
        }
    }
}