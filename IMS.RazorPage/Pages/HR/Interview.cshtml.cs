using IMS.Repositories.Enums;
using IMS.Repositories.Models.AccountModel;
using IMS.Repositories.Models.AssignmentModels;
using IMS.Repositories.Models.InternModel;
using IMS.Repositories.Models.InterviewModel;
using IMS.Services.Interfaces;
using IMS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace IMS.RazorPage.Pages.HR
{
    public class InterviewModel : PageModel
    {
        private readonly IInterviewService _interviewService;

        public List<InterviewGetModel> Interviews { get; set; }
        [BindProperty(SupportsGet = true)]
        public InterviewFilterModel filterModel { get; set; } = new InterviewFilterModel();

        [BindProperty]
        public InterviewStatus InterviewStatus { get; set; }
        [BindProperty]
        public Guid InterviewId { get; set; }

        public int TotalAccounts { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public InterviewModel(IInterviewService interviewService)
        {
            _interviewService = interviewService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier);
            filterModel.PageSize = PageSize;
            filterModel.PageNumber = PageNumber;
            filterModel.Search = SearchTerm;
            TotalAccounts = await _interviewService.GetTotalInterviewsCount(filterModel);
            Interviews = await _interviewService.GetAllInterview(filterModel);
            ViewData["Interviews"] = Interviews;
            ViewData["TotalAccountsCount"] = TotalAccounts;
            return Page();
        }

        public async Task<IActionResult> OnPostEditStatusAsync(Guid id)
        {

                var interview = await _interviewService.Get(id); // Fetch interview from service or repository
                if (interview == null)
                {
                    TempData["ErrorMessage"] = "Intern not found!";
                    return RedirectToPage("./Interview");
                }

                interview.Status = InterviewStatus; // Update the status

                if (await _interviewService.Update(interview))
                {
                    TempData["SuccessMessage"] = "Update successfully!";
                    return RedirectToPage("./Interview");
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong!";
                }
                // Optionally, add a success message or redirect to a confirmation page
                return RedirectToPage("./Interview");
        }
    }
}
