using IMS.Models.Entities;
using IMS.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Models.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly AppDbContext _dbContext;

        public AccountRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account> GetAccountByMail (string email)
        {
            return await _dbContext.Accounts.SingleOrDefaultAsync(a => a.Email.Equals(email));
        }
    }
}
