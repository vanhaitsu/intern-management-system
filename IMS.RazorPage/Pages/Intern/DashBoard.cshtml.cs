using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using IMS.Repositories.Models.AssignmentModels;
using IMS.Repositories.Models.FeedbackModel;
using IMS.Services.Interfaces;
using IMS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace IMS.RazorPage.Pages.Intern
{
    public class DashBoardModel : PageModel
    {
        private readonly IAssignmentService _assignmentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFeedbackService _feedbackService;

        [BindProperty]
        public Guid InternId { get; set; }
        public List<AssignmentViewModel> Assignments { get; set; }
        public List<TrainingProgram> TrainingPrograms { get; set; } = new List<TrainingProgram>();
        public List<Interview> Interviews { get; set; }
        public List<Feedback> Feedbacks { get; set; }

        public DashBoardModel(IAssignmentService assignmentService, IUnitOfWork unitOfWork, IFeedbackService feedbackService)
        {
            _assignmentService = assignmentService;
            _unitOfWork = unitOfWork;
            _feedbackService = feedbackService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            InternId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (InternId == null)
            {

                TempData["ErrorMessage"] = "InternId not found in claims.";
                return RedirectToPage("/Error");
            }



            Interviews = await _unitOfWork.InterviewRepository.GetInterviewsByInternId(InternId);
            Assignments = await _assignmentService.GetAssignmentsByInternId(InternId);
            Feedbacks = await _unitOfWork.FeedbackRepository.GetFeedbacksByInternId(InternId);
            foreach (var assignment in Assignments)
            {
                if (assignment.TrainingProgram != null && !TrainingPrograms.Contains(assignment.TrainingProgram))
                    TrainingPrograms.Add(assignment.TrainingProgram);
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAddFeedbackAsync(Guid trainingProgramId, string feedback)
        {
            InternId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Ensure InternId is set

            var feedbackCreateModel = new FeedbackCreateModel
            {
                TraningProgramId = trainingProgramId,
                InternID = InternId,
                commnent = feedback,
                MentorId = _unitOfWork.TrainingProgramRepository.GetCreateByGuid(trainingProgramId) ?? Guid.Empty,
                CreatedBy = InternId // Assuming CreatedBy should be InternId
            };

            var result = await _feedbackService.Create(feedbackCreateModel);

            if (result)
            {
                TempData["SuccessMessage"] = "Feedback added successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to add feedback.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteFeedbackAsync(Guid fbId)
        {
             var result = await _feedbackService.DeleteFeedback(fbId);
            if (result)
            {
                TempData["SuccessMessage"] = "Feedback deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete feedback.";
            }
            return RedirectToPage("/Intern/Dashboard");
                
        
        
        }

        public string GetNameById(Guid? id)
        {
           return   _unitOfWork.AccountRepository.getNamebyId(id);
           
        }
 
            
        }
    }

