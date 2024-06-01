using AutoMapper;
using IMS.Models.Entities;
using IMS.Models.Interfaces;
using IMS_VIew.Services.Interfaces;

namespace IMS_VIew.Services.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Role> GetByName(string name)
        {
            Role role = await _unitOfWork.RoleRepository.GetByName(name);
            if (role != null)
            {
                return role;
            }
            return null;
        }
    }
}
