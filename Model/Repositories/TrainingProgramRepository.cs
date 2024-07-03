using IMS.Models;
using IMS.Models.Entities;
using IMS.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Repositories
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
            return await _dbContext.TrainingPrograms.Where(tp => tp.AccountId == accountId).ToListAsync();
        }

        public async Task<TrainingProgram> GetByCode(string code)
        {
            return await _dbContext.TrainingPrograms.SingleOrDefaultAsync(tp => tp.Code == code);
        }
    }
}
