using AutoMapper;
using IMS.Repositories.Entities;
using IMS.Repositories.Enums;
using IMS.Repositories.Interfaces;
using IMS.Repositories.Models.ApplicationModel;
using IMS.Repositories.Models.CampaignModel;
using IMS.Repositories.Models.InternModel;
using IMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Services.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CampaignService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Create(CampaignAddModel campaignAddModel)
        {
            Campaign campaign = _mapper.Map<Campaign>(campaignAddModel);
            campaign.IsDeleted = false;
            await _unitOfWork.CampaignRepository.AddAsync(campaign);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var existedCampaign = await _unitOfWork.CampaignRepository.GetAsync(id);
            if (existedCampaign != null)
            {
                _unitOfWork.CampaignRepository.SoftDelete(existedCampaign);
            }
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<CampaignGetModel>> GetAllCampaigns(CampaignFilterModel filterModel)
        {
            var campaignList = await _unitOfWork.CampaignRepository.GetAllAsync(
                 filter: x =>
                    x.IsDeleted == false &&
                     (string.IsNullOrEmpty(filterModel.Search) || x.Name.ToLower().Contains(filterModel.Search.ToLower()) ||
                      x.Description.ToLower().Contains(filterModel.Search.ToLower())),
                 orderBy: x => filterModel.OrderByDescending
                     ? x.OrderByDescending(x => x.CreationDate)
                     : x.OrderBy(x => x.CreationDate),
                 pageIndex: filterModel.PageNumber,
                 pageSize: filterModel.PageSize
             );

            List<CampaignGetModel> campaignDetailList = null;

            if (campaignList != null)
            {
                campaignDetailList = campaignList.Data.Select(x => new CampaignGetModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsDelete = x.IsDeleted
                }).ToList();
            }

            return campaignDetailList ?? new List<CampaignGetModel>();
        }

        public async Task<Campaign> GetCampaignAsync(Guid id)
        {
            var campaign = await _unitOfWork.CampaignRepository.GetAsync(id);
            if (campaign != null)
            {
                return campaign;
            }
            return null;
        }

        public async Task<int> GetTotalCampaignsCount(CampaignFilterModel filterModel)
        {
            IQueryable<Campaign> query = _unitOfWork.CampaignRepository.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(filterModel.Search))
            {
                var search = filterModel.Search.ToLower();
                query = query.Where(a =>
                    a.Name.ToLower().Contains(search) ||
                    a.Description.ToLower().Contains(search)
                );
            }

            return await query.CountAsync();
        }

        public async Task<bool> Active(Guid id)
        {
            var existedCampaign = await _unitOfWork.CampaignRepository.GetAsync(id);
            if (existedCampaign != null)
            {
                existedCampaign.IsDeleted = false;
                _unitOfWork.CampaignRepository.Update(existedCampaign);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> Update(Guid id, CampaignUpdateModel updateModel)
        {
            var existedCampaign = await _unitOfWork.CampaignRepository.GetAsync(id);
            if (existedCampaign != null)
            {
                _mapper.Map(updateModel, existedCampaign);
                _unitOfWork.CampaignRepository.Update(existedCampaign);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<List<CampaignGetModel>> GetAllAvailableCampaigns(CampaignFilterModel filterModel)
        {
            var campaignList = await _unitOfWork.CampaignRepository.GetAllAsync(
                filter: x =>
                    (x.IsDeleted == false) &&
                    (string.IsNullOrEmpty(filterModel.Search) || x.Name.ToLower().Contains(filterModel.Search.ToLower()) ||
                        x.Description.ToLower().Contains(filterModel.Search.ToLower())),
            orderBy: x => filterModel.OrderByDescending
                    ? x.OrderByDescending(x => x.CreationDate)
                    : x.OrderBy(x => x.CreationDate),
            pageIndex: filterModel.PageNumber,
            pageSize: filterModel.PageSize
            );


            List<CampaignGetModel> campaignDetailList = null;

            if (campaignList != null)
            {
                campaignDetailList = campaignList.Data.Select(x => new CampaignGetModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsDelete = x.IsDeleted
                }).ToList();
            }

            return campaignDetailList ?? new List<CampaignGetModel>();
        }
        public async Task<bool> ApplyCampaign(ApplicationAddModel addModel)
        {
            var existedApplication = await _unitOfWork.ApplicationRepository.GetByInternIdAndCampaignId(addModel.InternId, addModel.CampaignId);
            if (existedApplication == null)
            {
                Application application = new Application
                {
                    InternId = addModel.InternId,
                    CampaignId = addModel.CampaignId,
                    Status = ApplicationStatus.Pending,
                    AppliedDate = DateTime.Now,
                    IsDeleted = false
                };
                await _unitOfWork.ApplicationRepository.AddAsync(application);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
