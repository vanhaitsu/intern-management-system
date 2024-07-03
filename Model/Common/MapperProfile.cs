﻿using AutoMapper;
using IMS.Models.Entities;
using Model.ViewModels.AccountModel;
using Model.ViewModels.TrainingProgramModel;

namespace IMS.Models.Common
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<AccountRegisterModel, Account>();
            CreateMap<AccountGetModel, Account>().ReverseMap();
            CreateMap<Account, AccountUpdateModel>().ForMember(dest => dest.RoleName, opt => opt.Ignore()).ReverseMap();

            CreateMap<TrainingProgramCreateModel, TrainingProgram>();
        }
    }
}
