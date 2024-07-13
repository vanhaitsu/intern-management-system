using IMS.Models.Repositories;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Repositories
{
    public class InterviewRepository : GenericRepository<Interview>, IInterviewRepository
    {
        private readonly AppDbContext _dbContext;
        public InterviewRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<List<Interview>> GetInterviewsByInternId(Guid internId)
        {
            return await _dbContext.Interviews.Where(x => x.InternId == internId).ToListAsync();
        }

        public IQueryable<Interview> GetAll()
        {
            return _dbContext.Interviews.Where(tp => tp.IsDeleted == false).AsQueryable();
        }
    }
}
