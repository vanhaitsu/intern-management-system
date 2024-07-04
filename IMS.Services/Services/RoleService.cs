using AutoMapper;
using IMS.Repositories.Entities;
using IMS.Repositories.Enums;
using IMS.Repositories.Interfaces;
using IMS.Services.Interfaces;

namespace IMS.Services.Services
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

        public async Task<List<Role>> GetAllRoles()
        {
            List<Role> roles = await _unitOfWork.RoleRepository.GetRoles();
            return roles;
        }
    }
}
