using IMS.Repositories.Entities;
using IMS.Repositories.Models.AssignmentModels;
using IMS.Repositories.Models.FeedbackModel;
using IMS.Repositories.Models.TrainingProgramModel;
using IMS.Services.Interfaces;
using IMS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace IMS.RazorPage.Pages.Mentor
{
    public class TrainingProgramDetailModel : PageModel
    {
        private readonly ITrainingProgramService _trainingProgramService;
        private readonly IAssignmentService _assignmentService;
        private readonly IInternService _internService;
        private readonly IFeedbackService _feedbackService;

        [BindProperty(SupportsGet = true)]
        public string TrainingProgramId { get; set; }
        public TrainingProgram TrainingProgram { set; get; }
        [BindProperty]
        public AssignmentCreateModel? AssignmentCreateModel { set; get; }
        [BindProperty]
        public AssignmentUpdateModel? AssignmentUpdateModel { set; get; }

        // Pagination
        public int TotalFeedbacks { get; set; }
        public int TotalAccounts { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public AssignmentFilterModel AssignmentFilterModel { get; set; }
        public List<Assignment> Assignments { get; set; }
        public FeedbackFilterModel FeedbackFilterModel { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        [BindProperty(SupportsGet = true)]
        public int FeedbackPageNumber { get; set; } = 1;

        public TrainingProgramDetailModel(ITrainingProgramService trainingProgramService, IAssignmentService assignmentService,
            IInternService internService, IFeedbackService feedbackService)
        {
            _trainingProgramService = trainingProgramService;
            _assignmentService = assignmentService;
            _internService = internService;
            _feedbackService = feedbackService;
        }

        public async Task<IActionResult> OnGet()
        {
            if (!string.IsNullOrEmpty(TrainingProgramId))
            {
                TrainingProgram = await _trainingProgramService.Get(Guid.Parse(TrainingProgramId));

                var queryResult = await _assignmentService.GetAssignments(new AssignmentFilterModel()
                {
                    Search = SearchTerm,
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    TrainingProgramId = TrainingProgram.Id
                });

                Assignments = queryResult.Data;
                TotalAccounts = queryResult.TotalCount;

                var feedBackQueryResult = await _feedbackService.GetFeedbacks(new FeedbackFilterModel()
                {
                    Search = SearchTerm,
                    PageNumber = FeedbackPageNumber,
                    PageSize = PageSize,
                    AccountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    TrainingProgramId = TrainingProgram.Id,
                });

                Feedbacks = feedBackQueryResult.Data;
                TotalFeedbacks = feedBackQueryResult.TotalCount;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAssignmentAsync()
        {
            ModelState.Remove("Name");
            ModelState.Remove("Style");
            ModelState.Remove("Duration");
            ModelState.Remove("StartDate");
            ModelState.Remove("Description");
            ModelState.Remove("Type");
            ModelState.Remove("TrainingProgramId");
            if (ModelState.IsValid)
            {
                AssignmentCreateModel.CreatedBy = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                if (AssignmentCreateModel.InternEmail != null)
                {
                    var intern = await _internService.GetByEmail(AssignmentCreateModel.InternEmail);
                    if (intern == null)
                    {
                        TempData["ErrorMessage"] = "Intern does not exist!";
                        return RedirectToPage("/Mentor/TrainingProgramDetail", new { trainingProgramId = TrainingProgramId });
                    }

                    AssignmentCreateModel.InternId = intern.Id;
                }

                if (await _assignmentService.Create(AssignmentCreateModel))
                {
                    TempData["SuccessMessage"] = "Add successfully!";
                    return RedirectToPage("/Mentor/TrainingProgramDetail", new { trainingProgramId = TrainingProgramId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong!";
                }
            }
            TempData["ErrorMessage"] = "Invalid input";
            return RedirectToPage("/Mentor/TrainingProgramDetail", new { trainingProgramId = TrainingProgramId });
        }

        public async Task<IActionResult> OnPostEditAssignmentAsync()
        {
            ModelState.Remove("Name");
            ModelState.Remove("Type");
            ModelState.Remove("Duration");
            ModelState.Remove("StartDate");
            ModelState.Remove("EndDate");
            ModelState.Remove("Description");
            ModelState.Remove("TrainingProgramId");
            if (ModelState.IsValid)
            {
                if (AssignmentUpdateModel.InternEmail != null)
                {
                    var intern = await _internService.GetByEmail(AssignmentUpdateModel.InternEmail);
                    if (intern == null)
                    {
                        TempData["ErrorMessage"] = "Intern does not exist!";
                        return RedirectToPage("/Mentor/TrainingProgramDetail", new { trainingProgramId = TrainingProgramId });
                    }

                    AssignmentUpdateModel.InternId = intern.Id;
                }

                if (await _assignmentService.Update(AssignmentUpdateModel))
                {
                    TempData["SuccessMessage"] = "Edit successfully!";
                    return RedirectToPage("/Mentor/TrainingProgramDetail", new { trainingProgramId = TrainingProgramId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong!";
                }
            }
            TempData["ErrorMessage"] = "Invalid input";
            return RedirectToPage("/Mentor/TrainingProgramDetail", new { trainingProgramId = TrainingProgramId });
        }

        public async Task<IActionResult> OnPostDeleteAssignmentAsync(Guid id)
        {
            if (await _assignmentService.Delete(id))
            {
                TempData["SuccessMessage"] = "Delete successfully!";
                return RedirectToPage("/Mentor/TrainingProgramDetail", new { trainingProgramId = TrainingProgramId });
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong!";
            }
            return RedirectToPage("/Mentor/TrainingProgramDetail", new { trainingProgramId = TrainingProgramId });
        }
    }
}
