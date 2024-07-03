using IMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IScoreService
    {

        Task<Score> GetByTraineeIdAsync(Guid trineeId);
        Task<List<Score>> GetByTrainingProgramCodeAsync(String code);

        Task<bool> DeleteScoreByIdAsync(Guid id);
        Task<bool> UpdateScoreAsync(Score score);
        Task<bool> CreateScoreAsync(Score score);



    }
}
