using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model.ViewModels.AccountModel;
using System.Security.Claims;
using IMS_View.Services.Interfaces;

namespace IMS.RazorPage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IAccountService _accountService;

        public string Message { set; get; }
        [BindProperty]
        public AccountLoginModel account { set; get; }

        public IndexModel(ILogger<IndexModel> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }
        
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync() 
        {
            AccountLoginModel accountModel = new();
            if (!ModelState.IsValid)
            {
                Message = "Email or password is wrong.\nPlease check again!";
                return Page();
            }
            else
            {
                accountModel = await _accountService.CheckLogin(account.Email, account.Password);
                if (accountModel == null)
                {
                    Message = "Email or password is not correct!";
                    return Page();
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
            else
            {
                return RedirectToPage("/Mentor/Trainee");
            }
        }

        //public async Task<IActionResult> Login(string email, string password)
        //{
        //    AccountLoginModel account = await _accountService.CheckLogin(email, password);
        //    if (account == null)
        //    {
        //        Message = "Email or password is wrong.\nPlease check again!";
        //        return Page();
        //    }
        //    else
        //    {
        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
        //            new Claim(ClaimTypes.Name, account.Email),
        //            new Claim(ClaimTypes.Role, account.Role)
        //        };

        //        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //        var principal = new ClaimsPrincipal(identity);
        //        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
        //            new AuthenticationProperties()
        //            {
        //                IsPersistent = true
        //            });
        //    }
        //    if (account.Role == "Admin")
        //    {
        //        return RedirectToPage("/Account");
        //    }
        //    else
        //    {
        //        return RedirectToPage("Index", "HR");
        //    }
        //}
    }
}
