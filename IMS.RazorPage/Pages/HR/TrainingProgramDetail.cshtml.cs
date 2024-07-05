//using IMS_View.Services.Interfaces;
//using IMS_VIew.Services.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Model.ViewModels.TraineeModel;
//using Newtonsoft.Json;
//using OfficeOpenXml;
//using System.Globalization;
//using System.Text;

//namespace IMS.RazorPage.Pages.Mentor
//{
//    public class TrainingProgramDetailModel : PageModel
//    {
//        private readonly ITraineeService _traineeService;
//        private readonly IRoleService _roleService;
//        private readonly ITrainingProgramService _trainingProgramService;

//        public TrainingProgramDetailModel(ITraineeService traineeService, ITrainingProgramService trainingProgramService)
//        {
//            _traineeService = traineeService;
//            _trainingProgramService = trainingProgramService;
//        }
//        public List<TraineeRegisterModel> UploadedTrainees { get; set; }
//        public string Message { set; get; }

//        [BindProperty(SupportsGet = true)]
//        public Guid ProgramId { get; set; }
//        public void OnGet()
//        {
//            if (Request.Query.ContainsKey("programId"))
//            {
//                var programIdString = Request.Query["programId"].ToString();
//                if (!string.IsNullOrEmpty(programIdString) && Guid.TryParse(programIdString, out Guid programId))
//                {
//                    ProgramId = programId;
//                    HttpContext.Session.SetString("ProgramId", ProgramId.ToString());
//                }
//            }
//        }

//        public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
//        {
//            if (file == null || file.Length <= 0)
//            {
//                return BadRequest("File not selected or empty.");
//            }
//            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
//            using (var stream = new MemoryStream())
//            {
//                await file.CopyToAsync(stream);
//                using (var package = new ExcelPackage(stream))
//                {
//                    var worksheet = package.Workbook.Worksheets[0];

//                    UploadedTrainees = new List<TraineeRegisterModel>();
//                    for (int row = worksheet.Dimension.Start.Row + 1; row <= worksheet.Dimension.End.Row; row++)
//                    {
//                        var dobString = worksheet.Cells[row, 7].GetValue<string>();
//                        DateTime dob;
//                        if (!DateTime.TryParseExact(dobString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
//                        {
//                            return BadRequest($"Invalid date format in row {row}. Expected format is dd-MM-yyyy.");
//                        }
//                        UploadedTrainees.Add(new TraineeRegisterModel
//                        {
//                            Code = worksheet.Cells[row, 1].GetValue<string>(),
//                            FullName = worksheet.Cells[row, 2].GetValue<string>(),
//                            Email = worksheet.Cells[row, 3].GetValue<string>(),
//                            Address = worksheet.Cells[row, 4].GetValue<string>(),
//                            University = worksheet.Cells[row, 5].GetValue<string>(),
//                            PhoneNumber = worksheet.Cells[row, 6].GetValue<string>(),
//                            DOB = dob,
//                            Gender = worksheet.Cells[row, 8].GetValue<string>(),
//                        });
//                    }
//                }
//            }
//            var json = JsonConvert.SerializeObject(UploadedTrainees);
//            var bytes = Encoding.UTF8.GetBytes(json);
//            HttpContext.Session.Set("UploadedTrainees", bytes);
//            return Page();
//        }

//        public async Task<IActionResult> OnPostConfirmAsync()
//        {
//            var programIdString = HttpContext.Session.GetString("ProgramId");
//            if (!string.IsNullOrEmpty(programIdString))
//            {
//                ProgramId = Guid.Parse(programIdString);
//            }

//            if (!ModelState.IsValid)
//            {
//                return Page();
//            }

//            var bytes = HttpContext.Session.Get("UploadedTrainees");
//            if (bytes != null)
//            {
//                var json = Encoding.UTF8.GetString(bytes);
//                UploadedTrainees = JsonConvert.DeserializeObject<List<TraineeRegisterModel>>(json);
//                var existingEmails = await _traineeService.GetAllTraineeEmails();
//                var emailSet = new HashSet<string>(existingEmails);

//                var traineesToUpdate = new List<TraineeRegisterModel>();
//                var traineesWithError = new List<TraineeRegisterModel>();

//                foreach (var trainee in UploadedTrainees)
//                {
//                    trainee.ProgramId = ProgramId;
//                    if (emailSet.Contains(trainee.Email))
//                    {
//                        traineesToUpdate.Add(trainee);
//                    }
//                    else
//                    {
//                        traineesWithError.Add(trainee);
//                    }
//                }
//                if (traineesToUpdate.Any())
//                {
//                        if (await _traineeService.UpdateRange(traineesToUpdate))
//                    {
//                        Message = "Add successfully!";
//                    }
//                    else
//                    {
//                        Message = "Something went wrong during update!";
//                    }
//                }

//                if (traineesWithError.Any())
//                {
//                    TempData["TraineesWithError"] = traineesWithError;
//                }
//            }
//            else
//            {
//                Message = "Uploaded trainees data not found in session.";
//            }

//            return Page();
//        }
//    }
//}
