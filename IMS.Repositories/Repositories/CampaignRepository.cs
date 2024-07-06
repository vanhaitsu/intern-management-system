using IMS.Models.Repositories;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;

namespace IMS.Repositories.Repositories
{
    public class CampaignRepository : GenericRepository<Campaign>, ICampaignRepository
    {
        private readonly AppDbContext _dbContext;
        public CampaignRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IQueryable<Campaign> GetAll()
        {
            return _dbContext.Campaigns.AsQueryable();
        }
    }
}
