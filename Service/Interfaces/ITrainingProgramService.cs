using IMS.Models.Entities;
using Model.ViewModels.TrainingProgramModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_View.Services.Interfaces
{
    public interface ITrainingProgramService
    {
        Task<List<TrainingProgram>> GetByAccount(Guid accountId);
        Task<TrainingProgram> GetByCode(string code);
        Task<bool> Create(TrainingProgramCreateModel trainingProgramCreateModel);
        Task<TrainingProgram> GetByName(string name);
        Task<List<TrainingProgram>> GetAllPrograms(int pageSize, int pageNumber);
    }
}
