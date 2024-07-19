using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

public class CheckAccountStatusMiddleware : IMiddleware
{
    private readonly IAccountService _accountService;
    private readonly IInternService _internService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CheckAccountStatusMiddleware(IAccountService accountService, IInternService internService, IHttpContextAccessor httpContextAccessor)
    {
        _accountService = accountService;
        _httpContextAccessor = httpContextAccessor;
        _internService = internService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var accountId = Guid.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var account = await _accountService.GetAccountAsync(accountId);
            var intern = await _internService.GetInternAsync(accountId);

            if ((account == null || account.IsDeleted) && intern == null)
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                context.Session.SetString("ErrorMessage", "Your account has been deleted. Please contact the administrator for more details.");
                context.Response.Redirect("/Index");
                return;
            }
        }
        await next(context);
    }
}
