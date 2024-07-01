using IMS.Models.Entities;
using IMS.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.ViewModels.AccountModel;

namespace IMS.Models.Repositories
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

        public async Task<List<AccountGetModel>> GetAllAccountsWithRole(int pageSize, int pageNumber)
        {
            var accountModels = await _dbContext.Accounts
            .Include(a => a.Role)
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
            RoleName = a.Role.Name // Access the Role navigation property
        })
        .Skip(pageNumber).Take(pageSize).ToListAsync();

            return accountModels;
        }
    }
}
