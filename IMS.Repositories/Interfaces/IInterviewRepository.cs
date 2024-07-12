using IMS.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Interfaces
{
    public interface IInterviewRepository : IGenericRepository<Interview>
    {

        Task<List<Interview>> GetInterviewsByInternId(Guid internId);
    }
}
