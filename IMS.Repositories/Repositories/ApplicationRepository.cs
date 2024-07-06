using IMS.Models.Repositories;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Application> GetByInternIdAndCampaignId(Guid internId,Guid campaignId)
        {
            return await _dbContext.Applications.SingleOrDefaultAsync(a => a.InternId == internId && a.CampaignId == campaignId);
        }


    }
}
