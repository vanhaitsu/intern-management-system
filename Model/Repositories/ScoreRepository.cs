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
    public class ScoreRepository : GenericRepository<Score>, IScoreRepository
    {
        private readonly AppDbContext _dbContext;

        public ScoreRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Score> GetByTraineeId(Guid traineeId)
        {
            return _dbContext.Scores.FirstOrDefaultAsync(s => s.TraineeId == traineeId);
        }
    }
}
