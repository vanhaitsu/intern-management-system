using IMS.Repositories.Entities;
using IMS.Repositories.Models.AccountModel;
using IMS.Repositories.Models.TrainingProgramModel;
using IMS.Services.Interfaces;
using IMS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Security.Claims;

namespace IMS.RazorPage.Pages.Mentor
{
    public class TrainingProgramListModel : PageModel
    {
        private readonly ITrainingProgramService _trainingProgramService;
        public List<TrainingProgramGetModel> TrainingPrograms { get; set; }
        public string ErrorMessage { set; get; }
        public string SuccessMessage { set; get; }
        [BindProperty]
        public TrainingProgramCreateModel TrainingProgramCreateModel { set; get; }
        [BindProperty]
        public TrainingProgramUpdateModel TrainingProgramUpdateModel { set; get; }
        [BindProperty(SupportsGet = true)]
        public AccountFilterModel filterModel { get; set; } = new AccountFilterModel();
        public int TotalAccounts { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 6;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public TrainingProgramListModel(ITrainingProgramService trainingProgramService)
        {
            _trainingProgramService = trainingProgramService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier);
            filterModel.PageSize = PageSize;
            filterModel.PageNumber = PageNumber;
            filterModel.Search = SearchTerm;
            TotalAccounts = await _trainingProgramService.GetTotalTrainingProgramsCount(filterModel, Guid.Parse(accountId.Value));
            TrainingPrograms = await _trainingProgramService.GetAllTrainingPrograms(filterModel, Guid.Parse(accountId.Value));
            ViewData["TrainingPrograms"] = TrainingPrograms;
            ViewData["TotalAccountsCount"] = TotalAccounts;
            return Page();
        }

        public async Task<IActionResult> OnPostCreateProgramAsync()
        {
            ModelState.Remove("Name");
            ModelState.Remove("SearchTerm");
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
            ModelState.Remove("SearchTerm");
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
