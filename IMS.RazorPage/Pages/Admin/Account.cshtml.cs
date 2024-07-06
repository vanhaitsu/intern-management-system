using IMS.Repositories.AccountModel;
using IMS.Repositories.Entities;
using IMS.Repositories.Models.AccountModel;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace IMS.RazorPage.Pages.Admin
{
    [Authorize]
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
        public List<AccountGetModel> Accounts { get; set; }
        public List<Role> Roles { get; set; }

        [BindProperty(SupportsGet = true)]
        public AccountFilterModel filterModel { get; set; } = new AccountFilterModel();
        public int TotalAccounts { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public AccountManagementModel(IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            filterModel.PageSize = PageSize;
            filterModel.PageNumber = PageNumber;
            filterModel.Search = SearchTerm;
            Accounts = await _accountService.GetAllAccounts(filterModel);
            Roles = await _roleService.GetAllRoles();
            TotalAccounts = await _accountService.GetTotalAccountsCount(filterModel);
            ViewData["Accounts"] = Accounts;
            ViewData["Roles"] = Roles;
            ViewData["TotalAccountsCount"] = TotalAccounts;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("SearchTerm");
            ModelState.Remove("Email");
            ModelState.Remove("Gender");
            ModelState.Remove("Address");
            ModelState.Remove("FullName");
            ModelState.Remove("PhoneNumber");

            if (!ModelState.IsValid)
            {
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        TempData["ModelStateError"] = error.ErrorMessage;
                    }
                }
                TempData["ToastMessage"] = "Validation errors occurred.";
                TempData["ToastType"] = "error";
                return RedirectToPage("./Account");
            }

            var updateModel = accountUpdate;
            var existedAccount = await _accountService.GetAccountAsync(id);

            if (existedAccount == null)
            {
                TempData["Error"] = "Account not found.";
                TempData["ToastMessage"] = "Account not found.";
                TempData["ToastType"] = "error";
                return RedirectToPage("./Account");
            }

            if (updateModel.Email != existedAccount.Email)
            {
                var emailExists = await _accountService.CheckExistedAccount(updateModel.Email);
                if (emailExists)
                {
                    TempData["Error"] = "Email is already existed!";
                    TempData["ToastMessage"] = "Email is already existed!";
                    TempData["ToastType"] = "error";
                    return RedirectToPage("./Account");
                }
            }

            if (await _accountService.Update(id, updateModel))
            {
                TempData["Message"] = "Update successfully!";
                TempData["ToastMessage"] = "Update successfully!";
                TempData["ToastType"] = "success";
                return RedirectToPage("./Account");
            }
            else
            {
                TempData["Error"] = "Failed to update!";
                TempData["ToastMessage"] = "Failed to update!";
                TempData["ToastType"] = "error";
                return RedirectToPage("./Account");
            }
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
            {
                var existedAccount = await _accountService.CheckExistedAccount(newAccount.Email);
                if (existedAccount)
                {
                    ViewData["Error"] = "Email is already existed!";
                    return Page();
                }
                if (await _accountService.Create(newAccount))
                {
                    Message = "Add successfully!";
                    return RedirectToPage("./Account");
                }
                else
                {
                    ViewData["Error"] = "Something went wrong!";
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var accountToDelete = await _accountService.GetAccountAsync(id);
            if (accountToDelete == null)
            {
                ViewData["Error"] = "Account not found.";
                return Page();
            }

            var deleteResult = await _accountService.Delete(id);
            if (!deleteResult)
            {
                ViewData["Error"] = "Failed to block account. Please try again.";
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
                ViewData["Error"] = "Account not found.";
                return Page();
            }

            var restoreResult = await _accountService.Restore(id);
            if (!restoreResult)
            {
                ViewData["Error"] = "Failed to Restore account. Please try again.";
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
