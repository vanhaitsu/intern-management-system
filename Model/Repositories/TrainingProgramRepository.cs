using IMS.Models.Entities;
using IMS.Models.Repositories;
using IMS.Models.Interfaces;
using IMS.Models;
using Microsoft.EntityFrameworkCore;

namespace IMS.Models.Repositories
{
    public class TrainingProgramRepository: GenericRepository<TrainingProgram>, ITrainingProgramRepository
    {
        private readonly AppDbContext _dbContext;

        public TrainingProgramRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TrainingProgram> GetByName(string name)
        {
            return await _dbContext.TrainingPrograms.FirstOrDefaultAsync(r => r.Name.Equals(name));
        }

        public async Task<TrainingProgram> GetByAccountId(Guid accountId)
        {
            return await _dbContext.TrainingPrograms.FirstOrDefaultAsync(r => r.AccountId == accountId);
        }
    }
}
