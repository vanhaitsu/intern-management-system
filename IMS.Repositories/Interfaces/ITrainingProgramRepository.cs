using IMS.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Interfaces
{
    public interface ITrainingProgramRepository : IGenericRepository<TrainingProgram>
    {
        Task<List<TrainingProgram>> GetByAccount(Guid accountId);
        IQueryable<TrainingProgram> GetAll();

        Guid? GetCreateByGuid(Guid id);
    }
}
