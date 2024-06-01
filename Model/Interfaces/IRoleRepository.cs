using IMS.Models.Entities;

namespace IMS_View.Models.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetByName(string name);
    }
}
