//using IMS.Models.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Model.ViewModels.TrainingProgramModel;
//using System.Security.Claims;
//using IMS_VIew.Services.Services;
//using IMS_View.Services.Interfaces;
//using System.Text;

//namespace IMS.RazorPage.Pages.Mentor
//{
//    public class TrainingProgramModel : PageModel
//    {
//        private readonly ITrainingProgramService _trainingProgramService;
//        public List<TrainingProgram> TrainingPrograms { get; set; }
//        public string ErrorMessage { set; get; }
//        public string SuccessMessage { set; get; }
//        [BindProperty]
//        public TrainingProgramCreateModel TrainingProgramCreateModel { set; get; }

//        public TrainingProgramModel(ITrainingProgramService trainingProgramService)
//        {
//            _trainingProgramService = trainingProgramService;
//        }
//        public async Task<IActionResult> OnGetAsync()
//        {
//            var accountId = User.FindFirst(ClaimTypes.NameIdentifier);
//            TrainingPrograms = await _trainingProgramService.GetByAccount(Guid.Parse(accountId.Value));
//            return Page();
//        }

//        public async Task<IActionResult> OnPostCreateProgramAsync()
//        {
//            if (!ModelState.IsValid)
//            {
//                var trainingProgram = await _trainingProgramService.GetByCode(TrainingProgramCreateModel.Code);
//                if (trainingProgram != null)
//                {
//                    ErrorMessage = "Code already existed. Please enter another one!";
//                    return Page();
//                }
//                TrainingProgramCreateModel.AccountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
//                if (await _trainingProgramService.Create(TrainingProgramCreateModel))
//                {
//                    SuccessMessage = "Add successfully!";
//                }
//                else
//                {
//                    ErrorMessage = "Something went wrong!";
//                }
//            }
//            return Page();
//        }
//        public IActionResult OnPostSaveProgramId(Guid programId)
//        {
//            HttpContext.Session.Set("ProgramId", Encoding.UTF8.GetBytes(programId.ToString()));
//            return Page();
//        }
//    }

//}
