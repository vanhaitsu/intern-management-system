using IMS.Models.Entities;
using IMS.Models.Interfaces;

namespace Model.Interfaces
{
    public interface ITrainingProgramRepository : IGenericRepository<TrainingProgram>
    {
        Task<List<TrainingProgram>> GetByAccount(Guid accountId);
        Task<TrainingProgram> GetByCode(string code);
    }
}
