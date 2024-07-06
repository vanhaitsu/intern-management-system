using IMS.Repositories.AccountModel;
using IMS.Repositories.Models.InternModel;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.RazorPage.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly ILogger<SignUpModel> _logger;
        private readonly IInternService _internService;
        public string Message { set; get; }

        [BindProperty]
        public InternRegisterModel intern { set; get; }

        public SignUpModel(IInternService internService, ILogger<SignUpModel> logger)
        {
            _internService = internService;
            _logger = logger;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (await _internService.SignUp(intern))
                {
                    Message = "Register successfully!";
                    return RedirectToPage("/Index");
                }
                else
                {
                    Message = "Something went wrong!";
                }   
            }
            return Page();
        }
    }
}
