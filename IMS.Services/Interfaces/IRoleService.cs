using IMS.Repositories.Entities;

namespace IMS.Services.Interfaces
{
    public interface IRoleService
    {
        Task<Role> GetByName(string name);
        Task<List<Role>> GetAllRoles();
    }
}
