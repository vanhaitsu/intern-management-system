using IMS.Repositories.Entities;
using IMS.Repositories.Models.AccountModel;
using IMS.Repositories.Models.InternModel;
using IMS.Services.Interfaces;
using IMS.Services.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace IMS.RazorPage.Pages.Common
{
    [Authorize]
    public class InternManagementModel : PageModel
    {
        private readonly IInternService _internService;

        public string Message { get; set; }
        [BindProperty]
        public InternRegisterModel intern { get; set; }
        [BindProperty]
        public InternRegisterModel newIntern { get; set; }
        [BindProperty]
        public InternUpdateModel internUpdate { get; set; }
        public List<Role> Roles { get; set; }
        public List<InternGetModel> interns { get; set; }
        public List<InternRegisterModel> UploadedInterns { get; set; }

        [BindProperty(SupportsGet = true)]
        public InternFilterModel filterModel { get; set; } = new InternFilterModel();
        public int TotalInterns { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public InternManagementModel(IInternService internService)
        {
            _internService = internService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            filterModel.PageSize = PageSize;
            filterModel.PageNumber = PageNumber;
            filterModel.Search = SearchTerm;
            interns = await _internService.GetAllInterns(filterModel);
            TotalInterns = await _internService.GetTotalInternsCount(filterModel);
            ViewData["Interns"] = interns;
            ViewData["TotalInternsCount"] = TotalInterns;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            RemoveModelStateErrors();
            if (!ModelState.IsValid)
                return ValidationFailedRedirect();

            var updateModel = internUpdate;
            var existedIntern = await _internService.GetInternAsync(id);

            if (existedIntern == null)
                return NotFoundRedirect("Intern not found.");

            if (updateModel.Email != existedIntern.Email)
            {
                var emailExists = await _internService.CheckExistedIntern(updateModel.Email);
                if (emailExists)
                    return EmailExistsRedirect("Email is already existed!");
            }

            return await UpdateInternAsync(id, updateModel);
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            RemoveModelStateErrors();
            if (!ModelState.IsValid)
                return ValidationFailedRedirect();

            var existedIntern = await _internService.CheckExistedIntern(newIntern.Email);
            if (existedIntern)
                return EmailExistsRedirect("Email is already existed!");

            return await CreateInternAsync(newIntern);
        }

        public async Task<IActionResult> OnPostBlockAsync(Guid id)
        {
            RemoveModelStateErrors();
            if (!ModelState.IsValid)
                return ValidationFailedRedirect();

            var internToDelete = await _internService.GetInternAsync(id);
            if (internToDelete == null)
                return NotFoundRedirect("Intern not found.");

            var deleteResult = await _internService.Delete(id);
            return deleteResult ? SuccessRedirect("Intern block successfully.") : ErrorRedirect("Failed to block intern. Please try again.");
        }

        public async Task<IActionResult> OnPostRestoreAsync(Guid id)
        {
            RemoveModelStateErrors();
            if (!ModelState.IsValid)
                return ValidationFailedRedirect();

            var internToDelete = await _internService.GetInternAsync(id);
            if (internToDelete == null)
                return NotFoundRedirect("Intern not found.");

            var restoreResult = await _internService.Restore(id);
            return restoreResult ? SuccessRedirect("Intern restore successfully.") : ErrorRedirect("Failed to restore intern. Please try again.");
        }

        public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return ErrorRedirect("File not selected or empty.");
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    UploadedInterns = new List<InternRegisterModel>();
                    for (int row = worksheet.Dimension.Start.Row + 1; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var dobString = worksheet.Cells[row, 6].GetValue<string>();
                        DateTime dob;
                        if (!DateTime.TryParseExact(dobString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
                        {
                            return ErrorRedirect($"Invalid date format in row {row}. Expected format is dd-MM-yyyy.");
                        }
                        UploadedInterns.Add(new InternRegisterModel
                        {
                            FullName = worksheet.Cells[row, 1].GetValue<string>(),
                            Email = worksheet.Cells[row, 2].GetValue<string>(),
                            Address = worksheet.Cells[row, 3].GetValue<string>(),
                            Education = worksheet.Cells[row, 4].GetValue<string>(),
                            PhoneNumber = worksheet.Cells[row, 5].GetValue<string>(),
                            DOB = dob,
                            Gender = worksheet.Cells[row, 7].GetValue<string>(),
                            Skill = worksheet.Cells[row, 8].GetValue<string>(),
                            WorkHistory = worksheet.Cells[row, 9].GetValue<string>(),
                        });
                    }
                }
            }
            var json = JsonConvert.SerializeObject(UploadedInterns);
            var bytes = Encoding.UTF8.GetBytes(json);

            HttpContext.Session.Set("Uploadedinterns", bytes);
            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync()
        {
            var bytes = HttpContext.Session.Get("Uploadedinterns");
            if (bytes != null)
            {
                var json = Encoding.UTF8.GetString(bytes);
                UploadedInterns = JsonConvert.DeserializeObject<List<InternRegisterModel>>(json);
                var existingEmails = await _internService.GetAllInternEmails();
                var emailSet = new HashSet<string>(existingEmails);

                foreach (var intern in UploadedInterns)
                {
                    if (emailSet.Contains(intern.Email))
                    {
                       return ErrorRedirect($"Email {intern.Email} is already existed!");
                       
                    }
                }
                if (await _internService.CreateRange(UploadedInterns))
                {
                   return SuccessRedirect("Add successfully!");
                }
                else
                {
                    ErrorRedirect("Something went wrong!");
                }
            }
            else
            {
                ErrorRedirect("Uploaded interns data not found in session.");
            }
            return RedirectToPage("./Intern");
        }

        private void RemoveModelStateErrors()
        {
            ModelState.Remove("Education");
            ModelState.Remove("Skill");
            ModelState.Remove("DOB");
            ModelState.Remove("Email");
            ModelState.Remove("Gender");
            ModelState.Remove("PhoneNumber");
            ModelState.Remove("FullName");
            ModelState.Remove("Address");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("SearchTerm");
            ModelState.Remove("Password");
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
            return RedirectToPage("./Intern");
        }

        private IActionResult NotFoundRedirect(string errorMessage)
        {
            TempData["Error"] = errorMessage;
            TempData["ToastMessage"] = errorMessage;
            TempData["ToastType"] = "error";
            return RedirectToPage("./Intern");
        }

        private IActionResult EmailExistsRedirect(string errorMessage)
        {
            TempData["Error"] = errorMessage;
            TempData["ToastMessage"] = errorMessage;
            TempData["ToastType"] = "error";
            return RedirectToPage("./Intern");
        }

        private async Task<IActionResult> UpdateInternAsync(Guid id, InternUpdateModel updateModel)
        {
            if (await _internService.Update(id, updateModel))
            {
                TempData["Message"] = "Update successfully!";
                TempData["ToastMessage"] = "Update successfully!";
                TempData["ToastType"] = "success";
                return RedirectToPage("./Intern");
            }
            else
            {
                TempData["Error"] = "Failed to update!";
                TempData["ToastMessage"] = "Failed to update!";
                TempData["ToastType"] = "error";
                return RedirectToPage("./Intern");
            }
        }

        private async Task<IActionResult> CreateInternAsync(InternRegisterModel newIntern)
        {
            if (await _internService.Create(newIntern))
            {
                TempData["Message"] = "Add successfully!";
                TempData["ToastMessage"] = "Add successfully!";
                TempData["ToastType"] = "success";
                return RedirectToPage("./Intern");
            }
            else
            {
                TempData["Error"] = "Failed to add!";
                TempData["ToastMessage"] = "Failed to add!";
                TempData["ToastType"] = "error";
                return RedirectToPage("./Intern");
            }
        }

        private IActionResult SuccessRedirect(string successMessage)
        {
            TempData["Message"] = successMessage;
            TempData["ToastMessage"] = successMessage;
            TempData["ToastType"] = "success";
            return RedirectToPage("./Intern");
        }

        private IActionResult ErrorRedirect(string errorMessage)
        {
            TempData["Error"] = errorMessage;
            TempData["ToastMessage"] = errorMessage;
            TempData["ToastType"] = "error";
            return RedirectToPage("./Intern");
        }
    }
}
