using AutoMapper;
using IMS.Models.Entities;
using IMS.Models.Interfaces;
using IMS_VIew.Services.Interfaces;
using Model.Enums;

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

        public async Task<List<Role>> GetAllRoles(int pageSize, int pageNumber)
        {
            if (pageSize == 0)
            {
                pageSize = int.Parse(Pagination.defaultPageSize.ToString());
            }
            if (pageNumber == 0)
            {
                pageNumber = int.Parse(Pagination.defaultPageNumber.ToString());
            }
            List<Role> roles = await _unitOfWork.RoleRepository.GetAllAsync();
            return roles;
        }
    }
}
