using IMS.Repositories.Interfaces;
namespace IMS.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITrainingProgramRepository _trainingProgramRepository;

        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, 
                        IRoleRepository roleRepository, ITrainingProgramRepository trainingProgramRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _trainingProgramRepository = trainingProgramRepository;
        }

        public AppDbContext DbContext => _dbContext;

        public IAccountRepository AccountRepository => _accountRepository;
        public IRoleRepository RoleRepository => _roleRepository;
        public ITrainingProgramRepository TrainingProgramRepository => _trainingProgramRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
