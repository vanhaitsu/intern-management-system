namespace IMS.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        AppDbContext DbContext { get; }
        IAccountRepository AccountRepository { get; }
        IRoleRepository RoleRepository {  get; }
        ITrainingProgramRepository TrainingProgramRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
