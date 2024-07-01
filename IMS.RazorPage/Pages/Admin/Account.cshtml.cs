using IMS.Models.Entities;
using IMS_View.Services.Interfaces;
using IMS_VIew.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model.ViewModels.AccountModel;

namespace IMS.RazorPage.Pages.Admin
{
    public class AccountManagementModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;

        public string Message { set; get; }
        [BindProperty]
        public AccountRegisterModel account { set; get; }
        [BindProperty]
        public AccountUpdateModel accountUpdate { set; get; }

        public AccountManagementModel(IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            List<AccountGetModel> accounts = await _accountService.GetAllAccounts();
            List<Role> roles = await _roleService.GetAllRoles(100,1);
            ViewData["Accounts"] = accounts;
            ViewData["Roles"] = roles;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            AccountUpdateModel updateModel = accountUpdate;
            return Page();
        }
    }
}
