using IMS.Repositories.Entities;
using IMS.Repositories.Models.InterviewModel;
using IMS.Repositories.Models.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Services.Interfaces
{
    public interface IInterviewService
    {
        Task<bool> Create(InterviewCreateModel interviewCreateModel);
        Task<List<InterviewGetModel>> GetAllInterview(InterviewFilterModel filterModel);
        Task<int> GetTotalInterviewsCount(InterviewFilterModel filterModel);
        Task<Interview> Get(Guid id);
        Task<bool> Update(Interview interview);
    }
}
