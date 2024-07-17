using IMS.Models.Repositories;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Repositories.Repositories
{
    public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
    {
        private readonly AppDbContext _dbContext;

        public AssignmentRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Assignment>> GetAssignmentsByInternId(Guid internId)
        {
           return await _dbContext.Assignments.Where(a => a.InternId == internId).Include(a => a.Intern).Include(a=> a.TrainingProgram).ToListAsync();
        }
    }
}
