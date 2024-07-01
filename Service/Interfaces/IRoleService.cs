using IMS.Models.Entities;

namespace IMS_VIew.Services.Interfaces
{
    public interface IRoleService
    {
        Task<Role> GetByName(string name);
        Task<List<Role>> GetAllRoles(int pageSize, int pageNumber);
    }
}
