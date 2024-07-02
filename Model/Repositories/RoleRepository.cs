using IMS.Models;
using IMS.Models.Entities;
using IMS.Models.Interfaces;
using IMS.Models.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IMS_View.Models.Repositories
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

    }
}
