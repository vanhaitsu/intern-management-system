using IMS.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<List<Application>> GetApplicationsByCampaignID(Guid campaignId);
        Task<bool> UpdateStatus(Guid applicationId, string status, Guid createdBNy);
        Task<Application> GetByInternIdAndCampaignId(Guid internId, Guid campaignId);
    }
}
