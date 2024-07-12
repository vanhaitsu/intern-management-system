using IMS.Models.Repositories;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Repositories.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly AppDbContext _dbContext;

        public RoleRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Role> GetByName(string name)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name.Equals(name));
        }

        public async Task<List<Role>> GetMentorsRole()
        {
            return await _dbContext.Roles.Where(r=> r.Name == "Mentor")
                                         .Include(r => r.Accounts).ToListAsync();
        }

        public async Task<List<Role>> GetRoles()
        {
            return await _dbContext.Roles.ToListAsync();
        }

    }
}
