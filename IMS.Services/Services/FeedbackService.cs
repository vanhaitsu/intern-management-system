using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using IMS.Repositories.Models.AssignmentModels;
using IMS.Repositories.Models.FeedbackModel;
using IMS.Repositories.QueryModels;
using IMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Services.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeedbackService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<QueryResultModel<List<Feedback>>> GetFeedbacks(FeedbackFilterModel feedbackFilterModel)
        {
            var feedbackList = await _unitOfWork.FeedbackRepository.GetAllAsync(
                x => x.IsDeleted == feedbackFilterModel.IsDeleted &&
                (feedbackFilterModel.InternId == null || x.InternId == feedbackFilterModel.InternId) &&
                (feedbackFilterModel.TrainingProgramId == null || x.TrainingProgramId == feedbackFilterModel.TrainingProgramId) &&
                (feedbackFilterModel.AccountId == null || x.AccountId == feedbackFilterModel.AccountId) &&
                (string.IsNullOrEmpty(feedbackFilterModel.Search)
                    || x.Intern.FullName.ToLower().Contains(feedbackFilterModel.Search.ToLower())
                    || x.Intern.Email.ToLower().Contains(feedbackFilterModel.Search.ToLower())
                    || x.TrainingProgram.Name.ToLower().Contains(feedbackFilterModel.Search.ToLower())
                    || x.Account.FullName.ToLower().Contains(feedbackFilterModel.Search.ToLower())
                    || x.Account.Email.ToLower().Contains(feedbackFilterModel.Search.ToLower())
                    || x.Comment.ToLower().Contains(feedbackFilterModel.Search.ToLower())),
                orderBy: x => feedbackFilterModel.OrderByDescending
                    ? x.OrderByDescending(x => x.CreationDate)
                    : x.OrderBy(x => x.CreationDate),
                pageIndex: feedbackFilterModel.PageNumber,
                pageSize: feedbackFilterModel.PageSize,
                includeProperties: "Intern, TrainingProgram, Account"
                );
            return feedbackList;
        }
    }
}
