using IMS.Repositories.AccountModel;
using IMS.Repositories.Entities;
using IMS.Repositories.Models.AccountModel;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            RemoveModelStateErrors();
            if (!ModelState.IsValid)
                return ValidationFailedRedirect();

            var updateModel = accountUpdate;
            var existedAccount = await _accountService.GetAccountAsync(id);

            if (existedAccount == null)
                return NotFoundRedirect("Account not found.");

            if (updateModel.Email != existedAccount.Email)
            {
                var emailExists = await _accountService.CheckExistedAccount(updateModel.Email);
                if (emailExists)
                    return EmailExistsRedirect("Email is already existed!");
            }

            return await UpdateAccountAsync(id, updateModel);
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            RemoveModelStateErrors();
            if (!ModelState.IsValid)
                return ValidationFailedRedirect();

            var existedAccount = await _accountService.CheckExistedAccount(newAccount.Email);
            if (existedAccount)
                return EmailExistsRedirect("Email is already existed!");

            return await CreateAccountAsync(newAccount);
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            RemoveModelStateErrors();
            if (!ModelState.IsValid)
                return ValidationFailedRedirect();

            var accountToDelete = await _accountService.GetAccountAsync(id);
            if (accountToDelete == null)
                return NotFoundRedirect("Account is not found!");

            var deleteResult = await _accountService.Delete(id);
            return deleteResult ? SuccessRedirect("Account block successfully.") : ErrorRedirect("Failed to block account. Please try again.");
        }

        public async Task<IActionResult> OnPostRestoreAsync(Guid id)
        {
            RemoveModelStateErrors();
            if (!ModelState.IsValid)
                return ValidationFailedRedirect();

            var accountToDelete = await _accountService.GetAccountAsync(id);
            if (accountToDelete == null)
                return NotFoundRedirect("Account is not found!");

            var restoreResult = await _accountService.Restore(id);
            return restoreResult ? SuccessRedirect("Account restore successfully.") : ErrorRedirect("Failed to restore account. Please try again.");
        }

        private void RemoveModelStateErrors()
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("SearchTerm");
            ModelState.Remove("Email");
            ModelState.Remove("Gender");
            ModelState.Remove("Address");
            ModelState.Remove("FullName");
            ModelState.Remove("PhoneNumber");
        }

        private IActionResult ValidationFailedRedirect()
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

        private IActionResult NotFoundRedirect(string errorMessage)
        {
            TempData["Error"] = errorMessage;
            TempData["ToastMessage"] = errorMessage;
            TempData["ToastType"] = "error";
            return RedirectToPage("./Account");
        }

        private IActionResult EmailExistsRedirect(string errorMessage)
        {
            TempData["Error"] = errorMessage;
            TempData["ToastMessage"] = errorMessage;
            TempData["ToastType"] = "error";
            return RedirectToPage("./Account");
        }

        private async Task<IActionResult> UpdateAccountAsync(Guid id, AccountUpdateModel updateModel)
        {
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

        private async Task<IActionResult> CreateAccountAsync(AccountRegisterModel newAccount)
        {
            if (await _accountService.Create(newAccount))
            {
                TempData["Message"] = "Add successfully!";
                TempData["ToastMessage"] = "Add successfully!";
                TempData["ToastType"] = "success";
                return RedirectToPage("./Account");
            }
            else
            {
                TempData["Error"] = "Failed to add!";
                TempData["ToastMessage"] = "Failed to add!";
                TempData["ToastType"] = "error";
                return RedirectToPage("./Account");
            }
        }

        private IActionResult SuccessRedirect(string successMessage)
        {
            TempData["Message"] = successMessage;
            TempData["ToastMessage"] = successMessage;
            TempData["ToastType"] = "success";
            return RedirectToPage("./Account");
        }

        private IActionResult ErrorRedirect(string errorMessage)
        {
            TempData["Error"] = errorMessage;
            TempData["ToastMessage"] = errorMessage;
            TempData["ToastType"] = "error";
            return RedirectToPage("./Account");
        }
    }
}
