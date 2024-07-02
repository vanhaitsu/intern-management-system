using IMS.Models.Entities;
using IMS_View.Services.Interfaces;
using IMS_VIew.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model.Enums;
using Model.ViewModels.TraineeModel;

namespace IMS.RazorPage.Pages.Mentor
{
    public class TraineeManagementModel : PageModel
    {
        private readonly ITraineeService _traineeService;
        private readonly IRoleService _roleService;

        public string Message { set; get; }
        [BindProperty]
        public TraineeRegisterModel trainee { set; get; }
        [BindProperty]
        public TraineeRegisterModel newTrainee { set; get; }
        [BindProperty]
        public TraineeUpdateModel traineeUpdate { set; get; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty]
        public int PageNumber { get; set; } = 1;

        [BindProperty]
        public int PageSize { get; set; } = 10;

        public TraineeManagementModel(ITraineeService traineeService, IRoleService roleService)
        {
            _traineeService = traineeService;
            _roleService = roleService;
        }

        public List<TraineeGetModel> Trainees { get; set; }
        public List<Role> Roles { get; set; }
        public int TotalTrainees { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Trainees = await _traineeService.GetAllTrainees(PageSize, PageNumber, SearchTerm);
            TotalTrainees = await _traineeService.GetTotalTraineesCount(SearchTerm);
            ViewData["Trainees"] = Trainees;
            ViewData["Roles"] = Roles;
            ViewData["TotalTraineesCount"] = TotalTrainees;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            TraineeUpdateModel updateModel = traineeUpdate;
            if (await _traineeService.Update(id, updateModel))
            {
                Message = "Update successfully!";
                return RedirectToPage("./TraineeManagement");
            }
            else
            {
                Message = "Failed to update!";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
            {
                if (await _traineeService.Create(newTrainee))
                {
                    Message = "Add successfully!";
                    return RedirectToPage("./TraineeManagement");
                }
                else
                {
                    Message = "Something went wrong!";
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var traineeToDelete = await _traineeService.GetTraineeAsync(id);
            if (traineeToDelete == null)
            {
                Message = "Trainee not found.";
                return Page();
            }

            var deleteResult = await _traineeService.Delete(id);
            if (!deleteResult)
            {
                Message = "Failed to delete trainee. Please try again.";
                return Page();
            }
            else
            {
                TempData["SuccessMessage"] = "Trainee deleted successfully.";
            }
            return RedirectToPage("./TraineeManagement");
        }
    }
}
