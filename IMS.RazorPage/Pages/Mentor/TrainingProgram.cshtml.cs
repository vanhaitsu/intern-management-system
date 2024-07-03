using IMS.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model.ViewModels.TrainingProgramModel;
using Service.Interfaces;
using System.Security.Claims;

namespace IMS.RazorPage.Pages.Mentor
{
    public class TrainingProgramModel : PageModel
    {
        private readonly ITrainingProgramService _trainingProgramService;
        public List<TrainingProgram> TrainingPrograms { get; set; }
        public string ErrorMessage { set; get; }
        public string SuccessMessage { set; get; }
        [BindProperty]
        public TrainingProgramCreateModel TrainingProgramCreateModel { set; get; }

        public TrainingProgramModel(ITrainingProgramService trainingProgramService)
        {
            _trainingProgramService = trainingProgramService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier);
            TrainingPrograms = await _trainingProgramService.GetByAccount(Guid.Parse(accountId.Value));
            return Page();
        }

        //[HttpPost]
        //public async Task<IActionResult> OnPostAsync(string action)
        //{
        //    switch (action)
        //    {
        //        case "create":
        //            return await CreateProgram();
        //        default:
        //            return Page();
        //    }
        //}

        public async Task<IActionResult> OnPostCreateProgramAsync()
        {
            if (!ModelState.IsValid)
            {
                var trainingProgram = await _trainingProgramService.GetByCode(TrainingProgramCreateModel.Code);
                if (trainingProgram != null)
                {
                    ErrorMessage = "Code already existed. Please enter another one!";
                    return Page();
                }
                TrainingProgramCreateModel.AccountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (await _trainingProgramService.Create(TrainingProgramCreateModel))
                {
                    SuccessMessage = "Add successfully!";
                }
                else
                {
                    ErrorMessage = "Something went wrong!";
                }
            }
            return Page();
        }
    }
}
