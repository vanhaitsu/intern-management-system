using AutoMapper;
using IMS.Repositories.AccountModel;
using IMS.Repositories.Entities;
using IMS.Repositories.Models.CampaignModel;
using IMS.Repositories.Models.InternModel;

namespace IMS.Repositories.Common
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<AccountRegisterModel, Account>();
            CreateMap<AccountGetModel, Account>().ReverseMap();
            CreateMap<Account, AccountUpdateModel>().ForMember(dest => dest.RoleName, opt => opt.Ignore()).ReverseMap();
            CreateMap<Intern, InternUpdateModel>().ReverseMap();
            CreateMap<InternRegisterModel, Intern>().ReverseMap();
            CreateMap<InternGetModel, Intern>().ReverseMap();
            CreateMap<CampaignAddModel,Campaign>().ReverseMap();
            CreateMap<Campaign, CampaignUpdateModel>().ReverseMap();
            //CreateMap<TrainingProgramCreateModel, TrainingProgram>();
        }
        private Guid? NormalizeProgramId(Guid? programId)
        {
            if (programId == Guid.Empty)
            {
                return null;
            }
            return programId;

        }
    }
}
