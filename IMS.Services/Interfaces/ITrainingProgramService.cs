using IMS.Repositories.Entities;
using IMS.Repositories.Models.AccountModel;
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
        Task<bool> SoftDelete(TrainingProgram trainingProgram);
        Task<bool> Update(TrainingProgramUpdateModel trainingProgramUpdateModel);
        Task<List<TrainingProgramGetModel>> GetAllTrainingPrograms(AccountFilterModel filterModel, Guid accountId);
        Task<int> GetTotalTrainingProgramsCount(AccountFilterModel filterModel, Guid accountId);
    }
}
