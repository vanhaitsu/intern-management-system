using IMS.Models.Interfaces;
using IMS_View.Models.Interfaces;
using Model.Interfaces;

namespace IMS.Models.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITraineeRepository _traineeRepository;

        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, 
                        IRoleRepository roleRepository, ITraineeRepository traineeRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _traineeRepository = traineeRepository;
        }

        public AppDbContext DbContext => _dbContext;

        public IAccountRepository AccountRepository => _accountRepository;
        public IRoleRepository RoleRepository => _roleRepository;

        public ITraineeRepository TraineeRepository => _traineeRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
