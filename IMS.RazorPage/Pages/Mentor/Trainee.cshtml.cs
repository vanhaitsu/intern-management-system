using IMS.Models.Entities;
using IMS_View.Services.Interfaces;
using IMS_VIew.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model.ViewModels.TraineeModel;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace IMS.RazorPage.Pages.Mentor
{
    public class TraineeModel : PageModel
    {
        private readonly ITraineeService _traineeService;
        private readonly IRoleService _roleService;
        private readonly ITrainingProgramService _trainingProgramService;

        public string Message { set; get; }
        [BindProperty]
        public TraineeRegisterModel trainee { set; get; }
        [BindProperty]
        public TraineeRegisterModel newTrainee { set; get; }
        [BindProperty]
        public TraineeUpdateModel traineeUpdate { set; get; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;


        public List<TrainingProgram> Programs { get; set; }
        public int TotalTrainees { get; set; }

        public TraineeModel(ITraineeService traineeService, ITrainingProgramService trainingProgramService)
        {
            _traineeService = traineeService;
            _trainingProgramService = trainingProgramService;
        }

        public List<TraineeGetModel> Trainees { get; set; }
        public List<TraineeRegisterModel> UploadedTrainees { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var accountIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(accountIdString, out Guid accountId))
            {

                Trainees = await _traineeService.GetTraineesByMentor(PageSize, PageNumber, SearchTerm, accountId);
                TotalTrainees = await _traineeService.GetTotal(SearchTerm, accountId);
                Programs = await _trainingProgramService.GetAllPrograms(100, 1);
                ViewData["Trainees"] = Trainees;
                ViewData["Programs"] = Programs;
                ViewData["TotalTraineesCount"] = TotalTrainees;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            TraineeUpdateModel updateModel = traineeUpdate;
            if (await _traineeService.Update(id, updateModel))
            {
                Message = "Update successfully!";
                return RedirectToPage("./Trainee");
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
                var existedAccount = await _traineeService.CheckExistedTrainee(newTrainee.Email);
                if (existedAccount)
                {
                    Message = "Email is already existed!";
                    return Page();
                }
                if (await _traineeService.Create(newTrainee))
                {
                    Message = "Add successfully!";
                    return RedirectToPage("./Trainee");
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
            var traineeToDelete = await _traineeService.GetTraineeAsync(id);
            if (traineeToDelete == null)
            {
                Message = "Trainee not found.";
                return Page();
            }

            var deleteResult = await _traineeService.Delete(id);
            if (!deleteResult)
            {
                Message = "Failed to delete trainee. Please try again.";
                return Page();
            }
            else
            {
                TempData["SuccessMessage"] = "Trainee deleted successfully.";
            }
            return RedirectToPage("./Trainee");
        }

        public async Task<IActionResult> OnPostRestoreAsync(Guid id)
        {
            var traineeToRestore = await _traineeService.GetTraineeAsync(id);
            if (traineeToRestore == null)
            {
                Message = "Trainee not found.";
                return Page();
            }

            var restoreResult = await _traineeService.Restore(id);
            if (!restoreResult)
            {
                Message = "Failed to Restore trainee. Please try again.";
                return Page();
            }
            else
            {
                TempData["SuccessMessage"] = "Trainee restore successfully.";
            }
            return RedirectToPage("./Trainee");
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

                    UploadedTrainees = new List<TraineeRegisterModel>();
                    for (int row = worksheet.Dimension.Start.Row + 1; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var dobString = worksheet.Cells[row, 6].GetValue<string>();
                        DateTime dob;
                        if (!DateTime.TryParseExact(dobString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
                        {
                            return BadRequest($"Invalid date format in row {row}. Expected format is dd-MM-yyyy.");
                        }
                        UploadedTrainees.Add(new TraineeRegisterModel
                        {
                            FullName = worksheet.Cells[row, 1].GetValue<string>(),
                            Email = worksheet.Cells[row, 2].GetValue<string>(),
                            Address = worksheet.Cells[row, 3].GetValue<string>(),
                            University = worksheet.Cells[row, 4].GetValue<string>(),
                            PhoneNumber = worksheet.Cells[row, 5].GetValue<string>(),
                            DOB = dob,
                            Gender = worksheet.Cells[row, 7].GetValue<string>(),
                        });
                    }
                }
            }
            var json = JsonConvert.SerializeObject(UploadedTrainees);
            var bytes = Encoding.UTF8.GetBytes(json);

            HttpContext.Session.Set("UploadedTrainees", bytes);
            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync()
        {
            if (!ModelState.IsValid)
            {
                var bytes = HttpContext.Session.Get("UploadedTrainees");
                if (bytes != null)
                {
                    var json = Encoding.UTF8.GetString(bytes);
                    UploadedTrainees = JsonConvert.DeserializeObject<List<TraineeRegisterModel>>(json);
                    var existingEmails = await _traineeService.GetAllTraineeEmails();
                    var emailSet = new HashSet<string>(existingEmails);

                    foreach (var trainee in UploadedTrainees)
                    {
                        if (emailSet.Contains(trainee.Email))
                        {
                            Message = $"Email {trainee.Email} is already existed!";
                            return Page();
                        }
                    }
                    if (await _traineeService.CreateRange(UploadedTrainees))
                    {
                        Message = "Add successfully!";
                        return RedirectToPage("./Trainee");
                    }
                    else
                    {
                        Message = "Something went wrong!";
                    }
                }
                else
                {
                    Message = "Uploaded trainees data not found in session.";
                }
            }
            return RedirectToPage("./Trainee");
        }
    }
}
