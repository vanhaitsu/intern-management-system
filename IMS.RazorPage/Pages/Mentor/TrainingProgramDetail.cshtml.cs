using IMS.Repositories.Entities;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.RazorPage.Pages.Mentor
{
    public class TrainingProgramDetailModel : PageModel
    {
        private readonly ITrainingProgramService _trainingProgramService;
        //public string TrainingProgramId { get; set; }
        public TrainingProgram TrainingProgram { set; get; }
        public TrainingProgramDetailModel(ITrainingProgramService trainingProgramService)
        {
            _trainingProgramService = trainingProgramService;
        }

        public async Task<IActionResult> OnGet(string name)
        {
            if (name != null)
            {
                TrainingProgram = await _trainingProgramService.Get(Guid.Parse(name));
            }
            return Page();
        }
    }
}
