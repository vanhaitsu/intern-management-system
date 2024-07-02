
namespace IMS.Models.Interfaces
{
    public interface IUnitOfWork
    {
        AppDbContext DbContext { get; }
        IAccountRepository AccountRepository { get; }
        IRoleRepository RoleRepository {  get; }
        ITraineeRepository TraineeRepository { get; }

        ITrainingProgramRepository TrainingProgramRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
