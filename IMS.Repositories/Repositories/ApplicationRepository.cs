using IMS.Models.Repositories;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace IMS.Repositories.Repositories
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationrepository
    {
        private readonly AppDbContext _dbContext;
        public ApplicationRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IQueryable<Application> GetAll()
        {
            return _dbContext.Applications.AsQueryable();
        }

        public  async Task<List<Application>> GetApplications(Guid campaignId)
        {
            return await _dbContext.Applications
                                   .Include(a => a.Intern)
                                   .Include(a => a.Campaign)
                                   .Where(a => a.CampaignId == campaignId && a.Status == 0 && a.IsDeleted == false).ToListAsync(); ;
        }

        public async Task<Application> GetByInternIdAndCampaignId(Guid internId,Guid campaignId)
        {
            return await _dbContext.Applications.SingleOrDefaultAsync(a => a.InternId == internId && a.CampaignId == campaignId);
        }


    }
}
