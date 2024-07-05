using IMS.Repositories.Entities;
using IMS.Repositories.Models.InternModel;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace IMS.RazorPage.Pages.Mentor
{
    public class InternModel : PageModel
    {
        private readonly IInternService _internService;
        private readonly IRoleService _roleService;
       // private readonly ITrainingProgramService _trainingProgramService;

        public string Message { set; get; }
        [BindProperty]
        public InternRegisterModel intern { set; get; }
        [BindProperty]
        public InternRegisterModel newIntern { set; get; }
        [BindProperty]
        public InternUpdateModel internUpdate { set; get; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;


        public List<TrainingProgram> Programs { get; set; }
        public int TotalInterns { get; set; }

        public InternModel(IInternService internService)
        {
            _internService = internService;
           // _trainingProgramService = trainingProgramService;
        }

        public List<InternGetModel> interns { get; set; }
        public List<InternRegisterModel> UploadedInterns { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var accountIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(accountIdString, out Guid accountId))
            {
                //interns = await _internService.GetInternsByMentor(PageSize, PageNumber, SearchTerm, accountId);
                //TotalInterns = await _internService.GetTotal(SearchTerm, accountId);
                //Programs = await _trainingProgramService.GetAllPrograms(100, 1);
                ViewData["interns"] = interns;
                ViewData["Programs"] = Programs;
                ViewData["TotalinternsCount"] = TotalInterns;
            }
            return Page();
        }

        //public async Task<IActionResult> OnPostAsync(Guid id)
        //{
        //    InternUpdateModel updateModel = internUpdate;
        //    if (await _internService.Update(id, updateModel))
        //    {
        //        Message = "Update successfully!";
        //        return RedirectToPage("./Intern");
        //    }
        //    else
        //    {
        //        Message = "Failed to update!";
        //        return Page();
        //    }
        //}

        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
            {
                var existedIntern = await _internService.CheckExistedIntern(newIntern.Email);
                if (existedIntern)
                {
                    Message = "Email is already existed!";
                    return Page();
                }
                if (await _internService.Create(newIntern))
                {
                    Message = "Add successfully!";
                    return RedirectToPage("./Intern");
                }
                else
                {
                    Message = "Something went wrong!";
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostBlockAsync(Guid id)
        {
            var internToDelete = await _internService.GetInternAsync(id);
            if (internToDelete == null)
            {
                Message = "intern not found.";
                return Page();
            }

            var deleteResult = await _internService.Delete(id);
            if (!deleteResult)
            {
                Message = "Failed to delete intern. Please try again.";
                return Page();
            }
            else
            {
                TempData["SuccessMessage"] = "Intern deleted successfully.";
            }
            return RedirectToPage("./Intern");
        }

        public async Task<IActionResult> OnPostRestoreAsync(Guid id)
        {
            var internToRestore = await _internService.GetInternAsync(id);
            if (internToRestore == null)
            {
                Message = "intern not found.";
                return Page();
            }

            var restoreResult = await _internService.Restore(id);
            if (!restoreResult)
            {
                Message = "Failed to Restore intern. Please try again.";
                return Page();
            }
            else
            {
                TempData["SuccessMessage"] = "intern restore successfully.";
            }
            return RedirectToPage("./Intern");
        }
    }
}
