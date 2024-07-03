using IMS.Models;
using IMS.Models.Entities;
using IMS.Models.Repositories;
using IMS.Models.Interfaces;
using IMS.Models;
using Microsoft.EntityFrameworkCore;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<List<TrainingProgram>> GetByAccount(Guid accountId)
        {
            return await _dbContext.TrainingPrograms.FirstOrDefaultAsync(r => r.Name.Equals(name));
            return await _dbContext.TrainingPrograms.Where(tp => tp.AccountId == accountId).ToListAsync();
        }

        public async Task<TrainingProgram> GetByCode(string code)
        public async Task<TrainingProgram> GetByAccountId(Guid accountId)
        {
            return await _dbContext.TrainingPrograms.SingleOrDefaultAsync(tp => tp.Code == code);
            return await _dbContext.TrainingPrograms.FirstOrDefaultAsync(r => r.AccountId == accountId);
        }
    }
}
