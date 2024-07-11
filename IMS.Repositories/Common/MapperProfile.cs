using AutoMapper;
using IMS.Repositories.AccountModel;
using IMS.Repositories.Entities;
using IMS.Repositories.Models.CampaignModel;
using IMS.Repositories.Models.TrainingProgramModel;
using IMS.Repositories.Models.InternModel;
using IMS.Repositories.Models.AssignmentModels;
using IMS.Repositories.Models.ApplicationModel;
using IMS.Repositories.Models.MentorshipModel;

namespace IMS.Repositories.Common
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AccountRegisterModel, Account>();
            CreateMap<AccountGetModel, Account>().ReverseMap();
            CreateMap<Account, AccountUpdateModel>().ForMember(dest => dest.RoleName, opt => opt.Ignore()).ReverseMap();

            CreateMap<TrainingProgramCreateModel, TrainingProgram>();
            CreateMap<TrainingProgramUpdateModel, TrainingProgram>();
            CreateMap<Intern, InternUpdateModel>().ReverseMap();
            CreateMap<InternRegisterModel, Intern>().ReverseMap();
            CreateMap<InternGetModel, Intern>().ReverseMap();
            CreateMap<CampaignAddModel,Campaign>().ReverseMap();
            CreateMap<Campaign, CampaignUpdateModel>().ReverseMap();
            //CreateMap<TrainingProgramCreateModel, TrainingProgram>();
            CreateMap<Mentorship, MentorshipCreateModel>().ReverseMap();
            CreateMap<AssignmentCreateModel, Assignment>();
            CreateMap<Application, ApplicationViewModel>()
                .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ApplicationName, opt => opt.MapFrom(src => src.Campaign.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CampaignId, opt => opt.MapFrom(src => src.CampaignId))
                .ForMember(dest => dest.Intern, opt => opt.MapFrom(src => src.Intern))
                .ForMember(dest => dest.ApplyDate, opt => opt.MapFrom(src => src.AppliedDate)).ReverseMap(); ;
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
