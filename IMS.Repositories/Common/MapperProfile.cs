using AutoMapper;
<<<<<<< HEAD:Model/Common/MapperProfile.cs
using IMS.Models.Entities;
using Model.ViewModels.AccountModel;
using Model.ViewModels.TrainingProgramModel;
using Model.ViewModels.TraineeModel;
=======
using IMS.Repositories.AccountModel;
using IMS.Repositories.Entities;
>>>>>>> origin/feature/hai:IMS.Repositories/Common/MapperProfile.cs

namespace IMS.Repositories.Common
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<AccountRegisterModel, Account>();
            CreateMap<AccountGetModel, Account>().ReverseMap();
            CreateMap<Account, AccountUpdateModel>().ForMember(dest => dest.RoleName, opt => opt.Ignore()).ReverseMap();
<<<<<<< HEAD:Model/Common/MapperProfile.cs
            CreateMap<Trainee, TraineeUpdateModel>().ForMember(dest => dest.ProgramName, opt => opt.Ignore()).ReverseMap();
            CreateMap<TraineeRegisterModel, Trainee>().ForMember(dest => dest.ProgramId, opt => opt.MapFrom(src => NormalizeProgramId(src.ProgramId))).ReverseMap();
            CreateMap<TraineeGetModel, Trainee>().ReverseMap();
        }
        private Guid? NormalizeProgramId(Guid? programId)
        {
            if (programId == Guid.Empty)
            {
                return null;
            }
            return programId;

            CreateMap<TrainingProgramCreateModel, TrainingProgram>();
=======
>>>>>>> origin/feature/hai:IMS.Repositories/Common/MapperProfile.cs
        }
    }
}
