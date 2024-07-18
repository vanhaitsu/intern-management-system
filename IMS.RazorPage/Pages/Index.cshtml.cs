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

        public IndexModel(ILogger<IndexModel> logger, IAccountService accountService, IInternService internService)
        {
            _logger = logger;
            _accountService = accountService;
            _internService = internService;
        }

        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToPage("/Admin/Account");
                }
                else if (User.IsInRole("HR"))
                {
                    return RedirectToPage("/HR/Campaign");
                }
                else if (User.IsInRole("Mentor"))
                {
                    return RedirectToPage("/Mentor/TrainingProgramList");
                }
                else
                {
                    return RedirectToPage("/Intern/Campaign");
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "Email or password is wrong.\nPlease check again!";
                return Page();
            }

            LoginResult loginResult;
            if (account.RoleCheck == "Staff")
            {
                loginResult = await _accountService.CheckLogin(account.Email, account.Password);
            }
            else
            {
                loginResult = await _internService.CheckLogin(account.Email, account.Password);
                if (loginResult.LoginModel != null)
                {
                    loginResult.LoginModel.Role = "Intern";
                }
            }

            if (!string.IsNullOrEmpty(loginResult.ErrorMessage))
            {
                ViewData["Error"] = loginResult.ErrorMessage;
                return Page();
            }

            var accountModel = loginResult.LoginModel;

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, accountModel.Id.ToString()),
        new Claim(ClaimTypes.Name, accountModel.Email)
    };

            if (!string.IsNullOrEmpty(accountModel.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role, accountModel.Role));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties()
                {
                    IsPersistent = true
                });

            if (accountModel.Role == "Admin")
            {
                return RedirectToPage("/Admin/Account");
            }
            else if (accountModel.Role == "HR")
            {
                return RedirectToPage("/HR/Campaign");
            }
            else if (accountModel.Role == "Mentor")
            {
                return RedirectToPage("/Mentor/TrainingProgramList");
            }
            else
            {
                return RedirectToPage("/Intern/Campaign");
            }
        }

    }
}
