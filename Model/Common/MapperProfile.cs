using AutoMapper;
using IMS.Models.Entities;
using IMS.Models.ViewModels;

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
