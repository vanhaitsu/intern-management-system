using IMS.Models.Interfaces;

namespace IMS.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        AppDbContext DbContext { get; }
        IAccountRepository AccountRepository { get; }
        IRoleRepository RoleRepository {  get; }
        IInternRepository InternRepository { get; }
        ICampaignRepository CampaignRepository { get; }
        IApplicationrepository ApplicationRepository { get; }

        public Task<int> SaveChangeAsync();
    }
}
