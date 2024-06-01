using AutoMapper;
using IMS.Models.Entities;
using Model.ViewModels.AccountModel;

namespace IMS.Models.Common
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<AccountRegisterModel, Account>();
        }
    }
}
