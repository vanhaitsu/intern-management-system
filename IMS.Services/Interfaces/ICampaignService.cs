

using IMS.Repositories.Entities;
using IMS.Repositories.Models.ApplicationModel;
using IMS.Repositories.Models.CampaignModel;
using IMS.Repositories.Models.InternModel;

namespace IMS.Services.Interfaces
{
    public interface ICampaignService
    {
        Task<List<CampaignGetModel>> GetAllCampaigns(CampaignFilterModel filterModel);
        Task<List<CampaignGetModel>> GetAllAvailableCampaigns(CampaignFilterModel filterModel,Guid internId);
        Task<Campaign> GetCampaignAsync(Guid id);
        Task<int> GetTotalCampaignsCount(CampaignFilterModel filterModel);
        Task<bool> Update(Guid id, CampaignUpdateModel updateModel);
        Task<bool> Create(CampaignAddModel campaignAddModel);
        Task<bool> Delete(Guid id);
        Task<bool> Active(Guid id);
        Task<bool> ApplyCampaign(ApplicationAddModel addModel);
    }
}
