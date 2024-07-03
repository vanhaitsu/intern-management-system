using IMS.Models.Entities;
using IMS.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Interfaces
{
    public interface IScoreRepository: IGenericRepository<Score> 
    {

        Task<Score> GetByTraineeId(Guid trineeId);

    }
}
