using IMS.Services.Interfaces;
using System.Security.Claims;

public class CheckAccountStatusMiddleware : IMiddleware
{
    private readonly IAccountService _accountService;

    public CheckAccountStatusMiddleware(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var accountId = Guid.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var account = await _accountService.GetAccountAsync(accountId);

            if (account == null || account.IsDeleted)
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("<script>window.location.href = '/Index';</script>");
                return;
            }
        }
        await next(context);
    }
}
