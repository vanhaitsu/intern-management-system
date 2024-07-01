using IMS_View.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model.ViewModels.AccountModel;

namespace IMS.RazorPage.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly ILogger<SignUpModel> _logger;
        private readonly IAccountService _accountService;
        public string Message { set; get; }

        [BindProperty]
        public AccountRegisterModel account { set; get; }

        public SignUpModel(IAccountService accountService, ILogger<SignUpModel> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (await _accountService.SignUp(account))
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
