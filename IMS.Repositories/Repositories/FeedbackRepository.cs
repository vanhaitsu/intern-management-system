using IMS.Models.Repositories;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Repositories
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        private readonly AppDbContext _dbContext;

        public FeedbackRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Feedback>> GetFeedbacksByInternId(Guid internId)
        {
            var result = await _dbContext.Feedbacks.ToListAsync();
            return await _dbContext.Feedbacks.Where(x => x.InternId == internId).ToListAsync();
        }
    }
}
