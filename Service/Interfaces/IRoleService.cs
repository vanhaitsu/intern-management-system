using IMS.Models.Entities;

namespace IMS_VIew.Services.Interfaces
{
    public interface IRoleService
    {
        Task<Role> GetByName(string name);
    }
}
