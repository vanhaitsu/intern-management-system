using IMS_View.Models.Interfaces;
using Model.Interfaces;

namespace IMS.Models.Interfaces
{
    public interface IUnitOfWork
    {
        AppDbContext DbContext { get; }
        IAccountRepository AccountRepository { get; }
        IRoleRepository RoleRepository {  get; }
        IScoreRepository ScoreRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
