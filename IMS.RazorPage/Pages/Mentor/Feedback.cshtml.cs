using IMS.Repositories.Entities;
using IMS.Repositories.Models.AssignmentModels;
using IMS.Repositories.Models.FeedbackModel;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace IMS.RazorPage.Pages.Mentor
{
    public class FeedbackModel : PageModel
    {
        private readonly ITrainingProgramService _trainingProgramService;
        private readonly IFeedbackService _feedbackService;

        public TrainingProgram TrainingProgram { set; get; }

        // Pagination
        public int TotalFeedbacks { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public FeedbackFilterModel FeedbackFilterModel { get; set; }
        public List<Feedback> Feedbacks { get; set; }

        public FeedbackModel(ITrainingProgramService trainingProgramService, IFeedbackService feedbackService)
        {
            _trainingProgramService = trainingProgramService;
            _feedbackService = feedbackService;
        }

        public async Task<IActionResult> OnGet(string name)
        {
            if (name != null)
            {
                TrainingProgram = await _trainingProgramService.Get(Guid.Parse(name));
            }

            var queryResult = await _feedbackService.GetFeedbacks(new FeedbackFilterModel()
            {
                Search = SearchTerm,
                PageNumber = PageNumber,
                PageSize = PageSize,
                AccountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                TrainingProgramId = Guid.Parse(name),
        });

            Feedbacks = queryResult.Data;
            TotalFeedbacks = queryResult.TotalCount;

            return Page();
        }
    }
}
