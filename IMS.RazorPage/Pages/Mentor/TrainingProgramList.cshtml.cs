using IMS.Repositories.Entities;
using IMS.Repositories.Models.TrainingProgramModel;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace IMS.RazorPage.Pages.Mentor
{
    public class TrainingProgramListModel : PageModel
    {
        private readonly ITrainingProgramService _trainingProgramService;
        public List<TrainingProgram> TrainingPrograms { get; set; }
        public string ErrorMessage { set; get; }
        public string SuccessMessage { set; get; }
        [BindProperty]
        public TrainingProgramCreateModel TrainingProgramCreateModel { set; get; }

        public TrainingProgramListModel(ITrainingProgramService trainingProgramService)
        {
            _trainingProgramService = trainingProgramService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier);
            TrainingPrograms = await _trainingProgramService.GetByAccount(Guid.Parse(accountId.Value));
            return Page();
        }

        public async Task<IActionResult> OnPostCreateProgramAsync()
        {
            if (ModelState.IsValid)
            {
                TrainingProgramCreateModel.CreatedBy = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (await _trainingProgramService.Create(TrainingProgramCreateModel))
                {
                    SuccessMessage = "Add successfully!";
                    return Page();
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
