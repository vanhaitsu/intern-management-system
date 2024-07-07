using IMS.Models.Interfaces;

namespace IMS.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        AppDbContext DbContext { get; }
        IAccountRepository AccountRepository { get; }
        IRoleRepository RoleRepository {  get; }
        ITrainingProgramRepository TrainingProgramRepository { get; }
        IInternRepository InternRepository { get; }
        IAssignmentRepository AssignmentRepository { get; }
        ICampaignRepository CampaignRepository { get; }
        IApplicationrepository ApplicationRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }

        public Task<int> SaveChangeAsync();
    }
}
