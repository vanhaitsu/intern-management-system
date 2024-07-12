using IMS.Models.Repositories;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Repositories.Repositories
{
    public class TrainingProgramRepository : GenericRepository<TrainingProgram>, ITrainingProgramRepository
    {
        private readonly AppDbContext _dbContext;

        public TrainingProgramRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TrainingProgram>> GetByAccount(Guid accountId)
        {
            return await _dbContext.TrainingPrograms.Where(tp => tp.CreatedBy == accountId &&
            tp.IsDeleted == false).ToListAsync();
        }

        public IQueryable<TrainingProgram> GetAll()
        {
            return _dbContext.TrainingPrograms.Where(tp => tp.IsDeleted == false).AsQueryable();
        }

        public  Guid? GetCreateByGuid(Guid id)
        {
            var tp = _dbContext.TrainingPrograms.Find(id);
            if(tp != null)
            {
                return tp.CreatedBy;
            }
            return null;
        }
    }
}
