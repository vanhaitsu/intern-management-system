using IMS.Repositories.Entities;
using IMS.Repositories.Models.AssignmentModels;
using IMS.Repositories.Models.TrainingProgramModel;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace IMS.RazorPage.Pages.Mentor
{
    public class TrainingProgramDetailModel : PageModel
    {
        private readonly ITrainingProgramService _trainingProgramService;
        private readonly IAssignmentService _assignmentService;
        //public string TrainingProgramId { get; set; }
        public TrainingProgram TrainingProgram { set; get; }
        public string ErrorMessage { set; get; }
        public string SuccessMessage { set; get; }
        [BindProperty]
        public AssignmentCreateModel AssignmentCreateModel { set; get; }

        // Pagination
        public int TotalAccounts { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public AssignmentFilterModel AssignmentFilterModel { get; set; }
        public List<Assignment> Assignments { get; set; }

        public TrainingProgramDetailModel(ITrainingProgramService trainingProgramService, IAssignmentService assignmentService)
        {
            _trainingProgramService = trainingProgramService;
            _assignmentService = assignmentService;
        }

        public async Task<IActionResult> OnGet(string name)
        {
            if (name != null)
            {
                TrainingProgram = await _trainingProgramService.Get(Guid.Parse(name));
            }

            var queryResult = await _assignmentService.GetAssignments(new AssignmentFilterModel()
            {
                Search = SearchTerm,
                PageNumber = PageNumber,
                PageSize = PageSize,
            });

            Assignments = queryResult.Data;
            TotalAccounts = queryResult.TotalCount;

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAssignmentAsync()
        {
            if (ModelState.IsValid)
            {
                AssignmentCreateModel.CreatedBy = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (await _assignmentService.Create(AssignmentCreateModel))
                {
                    SuccessMessage = "Add successfully!";
                    return RedirectToPage("/Mentor/TrainingProgramDetail", new { name = AssignmentCreateModel.TrainingProgramId });
                }
                else
                {
                    ErrorMessage = "Something went wrong!";
                }
            }
            return null;
        }
    }
}
