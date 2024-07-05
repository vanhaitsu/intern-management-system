using IMS.Models.Interfaces;

namespace IMS.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        AppDbContext DbContext { get; }
        IAccountRepository AccountRepository { get; }
        IRoleRepository RoleRepository {  get; }
<<<<<<< HEAD
        ITrainingProgramRepository TrainingProgramRepository { get; }
=======
        IInternRepository InternRepository { get; }

>>>>>>> origin/main
        public Task<int> SaveChangeAsync();
    }
}
