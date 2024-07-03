using IMS.Models.Interfaces;


namespace IMS.Models.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITraineeRepository _traineeRepository;
        private readonly ITrainingProgramRepository _trainingProgramRepository;

        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, 
                        IRoleRepository roleRepository, ITraineeRepository traineeRepository, 
                        ITrainingProgramRepository trainingProgramRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _traineeRepository = traineeRepository;
            _trainingProgramRepository = trainingProgramRepository;
        }

        public AppDbContext DbContext => _dbContext;

        public IAccountRepository AccountRepository => _accountRepository;
        public IRoleRepository RoleRepository => _roleRepository;
        public ITraineeRepository TraineeRepository => _traineeRepository;
        public ITrainingProgramRepository TrainingProgramRepository => _trainingProgramRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
