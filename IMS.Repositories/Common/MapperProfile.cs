using AutoMapper;
using IMS.Repositories.AccountModel;
using IMS.Repositories.Entities;

namespace IMS.Repositories.Common
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<AccountRegisterModel, Account>();
            CreateMap<AccountGetModel, Account>().ReverseMap();
            CreateMap<Account, AccountUpdateModel>().ForMember(dest => dest.RoleName, opt => opt.Ignore()).ReverseMap();
        }
    }
}
