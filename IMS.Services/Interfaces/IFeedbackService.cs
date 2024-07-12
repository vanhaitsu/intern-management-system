using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using IMS.Repositories.Models.FeedbackModel;
using IMS.Repositories.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<QueryResultModel<List<Feedback>>> GetFeedbacks(FeedbackFilterModel feedbackFilterModel);
        Task<bool> Create(FeedbackCreateModel feedbackCreateModel);
       
      
    }
}
