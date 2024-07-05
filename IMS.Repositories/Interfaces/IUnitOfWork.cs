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
        public Task<int> SaveChangeAsync();
    }
}
