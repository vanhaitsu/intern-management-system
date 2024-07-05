using IMS.Repositories.Entities;
using IMS.Repositories.Models.AccountModel;
using IMS.Repositories.Models.InternModel;
using IMS.Services.Interfaces;
using IMS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace IMS.RazorPage.Pages
{
    public class InternManagementModel : PageModel
    {
        private readonly IInternService _internService;

        public string Message { set; get; }
        [BindProperty]
        public InternRegisterModel intern { set; get; }
        [BindProperty]
        public InternRegisterModel newIntern { set; get; }
        [BindProperty]
        public InternUpdateModel internUpdate { set; get; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public int TotalInterns { get; set; }
        public List<InternGetModel> interns { get; set; }
        public List<InternRegisterModel> UploadedInterns { get; set; }

        [BindProperty(SupportsGet = true)]
        public InternFilterModel filterModel { get; set; } = new InternFilterModel();

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
            InternUpdateModel updateModel = internUpdate;
            var existedIntern = await _internService.GetInternAsync(id);
            if (existedIntern == null)
            {
                Message = "Intern not found.";
                return Page();
            }
            if (updateModel.Email != existedIntern.Email)
            {
                var emailExists = await _internService.CheckExistedIntern(updateModel.Email);
                if (emailExists)
                {
                    Message = "Email is already existed!";
                    return Page();
                }
            }
            if (await _internService.Update(id, updateModel))
            {
                Message = "Update successfully!";
                return RedirectToPage("./Intern");
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
                var existedIntern = await _internService.CheckExistedIntern(newIntern.Email);
                if (existedIntern)
                {
                    Message = "Email is already existed!";
                    return Page();
                }
                if (await _internService.Create(newIntern))
                {
                    Message = "Add successfully!";
                    return RedirectToPage("./Intern");
                }
                else
                {
                    Message = "Something went wrong!";
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostBlockAsync(Guid id)
        {
            var internToDelete = await _internService.GetInternAsync(id);
            if (internToDelete == null)
            {
                Message = "intern not found.";
                return Page();
            }

            var deleteResult = await _internService.Delete(id);
            if (!deleteResult)
            {
                Message = "Failed to delete intern. Please try again.";
                return Page();
            }
            else
            {
                TempData["SuccessMessage"] = "Intern deleted successfully.";
            }
            return RedirectToPage("./Intern");
        }

        public async Task<IActionResult> OnPostRestoreAsync(Guid id)
        {
            var internToRestore = await _internService.GetInternAsync(id);
            if (internToRestore == null)
            {
                Message = "intern not found.";
                return Page();
            }

            var restoreResult = await _internService.Restore(id);
            if (!restoreResult)
            {
                Message = "Failed to Restore intern. Please try again.";
                return Page();
            }
            else
            {
                TempData["SuccessMessage"] = "intern restore successfully.";
            }
            return RedirectToPage("./Intern");
        }

        public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("File not selected or empty.");
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
                            return BadRequest($"Invalid date format in row {row}. Expected format is dd-MM-yyyy.");
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
            if (!ModelState.IsValid)
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
                            Message = $"Email {intern.Email} is already existed!";
                            return Page();
                        }
                    }
                    if (await _internService.CreateRange(UploadedInterns))
                    {
                        Message = "Add successfully!";
                        return RedirectToPage("./Intern");
                    }
                    else
                    {
                        Message = "Something went wrong!";
                    }
                }
                else
                {
                    Message = "Uploaded interns data not found in session.";
                }
            }
            return RedirectToPage("./Intern");
        }
    }
}
