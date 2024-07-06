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
        [BindProperty]
        public TrainingProgramUpdateModel TrainingProgramUpdateModel { set; get; }

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
            ModelState.Remove("Name");
            if (ModelState.IsValid)
            {
                TrainingProgramCreateModel.CreatedBy = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (await _trainingProgramService.Create(TrainingProgramCreateModel))
                {
                    SuccessMessage = "Add successfully!";
                    return RedirectToPage("./TrainingProgramList");
                }
                else
                {
                    ErrorMessage = "Something went wrong!";
                }
            }
            return null;
        }

        public async Task<IActionResult> OnPostDeleteProgramAsync(Guid id)
        {
            var trainingProgram = await _trainingProgramService.Get(id);
            if (trainingProgram == null)
            {
                ErrorMessage = "Training Program not found.";
                return Page();
            }
            
            if (await _trainingProgramService.SoftDelete(trainingProgram))
            {
                SuccessMessage = "Delete successfully!";
            }
            else
            {
                ErrorMessage = "Something went wrong!";
            }
            return RedirectToPage("./TrainingProgramList");
        }

        public async Task<IActionResult> OnPostEditProgramAsync()
        {
            ModelState.Remove("Name");
            if (ModelState.IsValid)
            {
                var trainingProgram = await _trainingProgramService.Get((Guid)TrainingProgramUpdateModel.Id);
                if (trainingProgram == null)
                {
                    ErrorMessage = "Training Program not found.";
                    return RedirectToPage("./TrainingProgramList");
                }

                if (await _trainingProgramService.Update(TrainingProgramUpdateModel))
                {
                    SuccessMessage = "Update successfully!";
                }
                else
                {
                    ErrorMessage = "Something went wrong!";
                }
            }
            return RedirectToPage("./TrainingProgramList");
        }
    }
}
