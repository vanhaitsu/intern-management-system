using IMS.Repositories.Entities;
using IMS.Repositories.Models.TrainingProgramModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Services.Interfaces
{
    public interface ITrainingProgramService
    {
        Task<List<TrainingProgram>> GetByAccount(Guid accountId);
        Task<TrainingProgram> Get(Guid id);
        Task<bool> Create(TrainingProgramCreateModel trainingProgramCreateModel);
    }
}
