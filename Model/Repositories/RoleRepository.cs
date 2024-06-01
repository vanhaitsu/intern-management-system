using IMS.Models;
using IMS.Models.Entities;
using IMS.Models.Repositories;
using IMS_View.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS_View.Models.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _dbContext;

        public RoleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Role> GetByName(string name)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name.Equals(name));
        }

    }
}
