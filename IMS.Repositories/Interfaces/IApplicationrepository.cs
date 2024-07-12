using IMS.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Interfaces
{
    public interface IApplicationrepository : IGenericRepository<Application>
    {
        IQueryable<Application> GetAll();
        public Task<Application> GetByInternIdAndCampaignId(Guid internId, Guid campaignId);

        public Task<List<Application>> GetApplications(Guid campaignId);
    }
}
