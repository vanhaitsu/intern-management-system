using IMS.Models.Entities;
using IMS.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.Interfaces
{
    public interface ITrainingProgramRepository : IGenericRepository<TrainingProgram>
    {
        Task<TrainingProgram> GetByName(string name);
        Task<TrainingProgram> GetByAccountId(Guid accountId);
    }
}
