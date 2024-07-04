﻿using IMS.Repositories.Entities;

namespace IMS.Repositories.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role> GetByName(string name);
    }
}
