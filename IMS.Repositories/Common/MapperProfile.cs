using AutoMapper;
using IMS.Repositories.AccountModel;
using IMS.Repositories.Entities;
using IMS.Repositories.Models.TrainingProgramModel;
using IMS.Repositories.Models.InternModel;
using IMS.Repositories.Models.AssignmentModels;

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
            CreateMap<Intern, InternUpdateModel>().ReverseMap();
            CreateMap<InternRegisterModel, Intern>().ReverseMap();
            CreateMap<InternGetModel, Intern>().ReverseMap();
            //CreateMap<TrainingProgramCreateModel, TrainingProgram>();

            CreateMap<AssignmentCreateModel, Assignment>();
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
