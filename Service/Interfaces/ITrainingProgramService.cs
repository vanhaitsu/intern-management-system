using IMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_View.Services.Interfaces
{
    public interface ITrainingProgramService
    {
        Task<TrainingProgram> GetByName(string name);
        Task<List<TrainingProgram>> GetAllPrograms(int pageSize, int pageNumber);
    }
}
