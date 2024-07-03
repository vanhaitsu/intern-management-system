using IMS.Models.Entities;
using IMS_View.Services.Interfaces;
using IMS_VIew.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model.Enums;
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

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty]
        public int PageNumber { get; set; } = 1;

        [BindProperty]
        public int PageSize { get; set; } = 10;

        public AccountManagementModel(IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
        }
        public List<AccountGetModel> Accounts { get; set; }
        public List<Role> Roles { get; set; }
        public int TotalAccounts { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Accounts = await _accountService.GetAllAccounts(PageSize, PageNumber, SearchTerm);
            Roles = await _roleService.GetAllRoles(100, 1);
            TotalAccounts = await _accountService.GetTotalAccountsCount(SearchTerm);
            ViewData["Accounts"] = Accounts;
            ViewData["Roles"] = Roles;
            ViewData["TotalAccountsCount"] = TotalAccounts;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {

            AccountUpdateModel updateModel = accountUpdate;
            var existedAccount =  await _accountService.CheckExistedAccount(updateModel.Email);
            if (existedAccount)
            {
                Message = "Email is already existed!";
                return Page();
            }
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
                var existedAccount = await _accountService.CheckExistedAccount(newAccount.Email);
                if (existedAccount)
                {
                    Message = "Email is already existed!";
                    return Page();
                }
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
                Message = "Failed to block account. Please try again.";
                return Page();
            }
            else
            {
                TempData["SuccessMessage"] = "Account block successfully.";
            }
            return RedirectToPage("./Account");
        }

        public async Task<IActionResult> OnPostRestoreAsync(Guid id)
        {
            var accountToDelete = await _accountService.GetAccountAsync(id);
            if (accountToDelete == null)
            {
                Message = "Account not found.";
                return Page();
            }

            var restoreResult = await _accountService.Restore(id);
            if (!restoreResult)
            {
                Message = "Failed to Restore account. Please try again.";
                return Page();
            }
            else
            {
                TempData["SuccessMessage"] = "Account restore successfully.";
            }
            return RedirectToPage("./Account");
        }

    }
}
