using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using IMS.Services.Interfaces;
using IMS.Repositories.AccountModel;
using IMS.Repositories.Models.InternModel;
using IMS.Repositories.Models.CommonModel;

namespace IMS.RazorPage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IAccountService _accountService;
        private readonly IInternService _internService;

        public string Message { get; set; }
        [BindProperty]
        public LoginModel account { get; set; }

        [BindProperty]
        public string role { get; set; }
        public IndexModel(ILogger<IndexModel> logger, IAccountService accountService, IInternService internService)
        {
            _logger = logger;
            _accountService = accountService;
            _internService = internService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            LoginModel accountModel = new();
            if (ModelState.IsValid)
            {
                Message = "Email or password is wrong.\nPlease check again!";
                return Page();
            }
            else
            {
                if (role == "Staff")
                {
                    accountModel = await _accountService.CheckLogin(account.Email, account.Password);
                    if (accountModel == null)
                    {
                        Message = "Email or password is not correct!";
                        return Page();
                    }

                }
                else
                {
                    accountModel = await _internService.CheckLogin(account.Email, account.Password);
                    if (accountModel == null)
                    {
                        Message = "Email or password is not correct!";
                        return Page();
                    }
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, accountModel.Id.ToString()),
                    new Claim(ClaimTypes.Name, accountModel.Email),
                    new Claim(ClaimTypes.Role, accountModel.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    new AuthenticationProperties()
                    {
                        IsPersistent = true
                    });
            }
            if (accountModel.Role == "Admin")
            {
                return RedirectToPage("/Admin/Account");
            }
            else if(accountModel.Role == "HR")
            {
                return RedirectToPage("/HR/Intern");
            }
            else if(accountModel.Role == "Mentor")
            {
                return RedirectToPage("/Mentor/TrainingProgramList");
            }
            else
            {
                return RedirectToPage("/Intern/Intern");
            }
        }
    }
}
