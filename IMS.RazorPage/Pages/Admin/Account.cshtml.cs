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
        public AccountRegisterModel newAccount { set; get; }
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
            List<Role> roles = await _roleService.GetAllRoles(100, 1);
            ViewData["Accounts"] = accounts;
            ViewData["Roles"] = roles;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {

            AccountUpdateModel updateModel = accountUpdate;
            if (await _accountService.Update(id, updateModel))
            {
                Message = "Update successfully!";
                return RedirectToPage("./Account");
            }
            else
            {
                Message = "Failed to update!";
                return Page();
            }

        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
            {
                if (await _accountService.Create(newAccount))
                {
                    Message = "Add successfully!";
                    return RedirectToPage("./Account");
                }
                else
                {
                    Message = "Something went wrong!";
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var accountToDelete = await _accountService.GetAccountAsync(id);
            if (accountToDelete == null)
            {
                Message = "Account not found.";
                return Page();
            }

            var deleteResult = await _accountService.Delete(id);
            if (!deleteResult)
            {
                Message = "Failed to block customer. Please try again.";
                return Page();
            }
            else
            {
                TempData["SuccessMessage"] = "Account block successfully.";
            }
            return RedirectToPage("./Account");
        }

    }
}
