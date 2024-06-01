using IMS_View.Models.Interfaces;

namespace IMS.Models.Interfaces
{
    public interface IUnitOfWork
    {
        AppDbContext DbContext { get; }
        IAccountRepository AccountRepository { get; }
        IRoleRepository RoleRepository {  get; }
        public Task<int> SaveChangeAsync();
    }
}
