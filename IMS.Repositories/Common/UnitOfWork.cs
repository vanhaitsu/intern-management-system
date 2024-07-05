
﻿using IMS.Models.Interfaces;
namespace IMS.Models.Common;

using IMS.Repositories;
using IMS.Repositories.Interfaces;
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
    private readonly IInternRepository _internRepository;

    public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, 
                        IRoleRepository roleRepository, IInternRepository internRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
        _internRepository = internRepository;
        }

        public AppDbContext DbContext => _dbContext;

        public IAccountRepository AccountRepository => _accountRepository;
        public IRoleRepository RoleRepository => _roleRepository;

    public IInternRepository InternRepository => _internRepository;



    public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }

