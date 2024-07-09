using AutoMapper;
using IMS.Repositories.Models.InternModel;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.RazorPage.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly ILogger<SignUpModel> _logger;
        private readonly IInternService _internService;
        private readonly IMapper _mapper;
        public string Message { set; get; }

        [BindProperty]
        public InternRegisterModel internRegister { set; get; }

        public SignUpModel(IInternService internService, ILogger<SignUpModel> logger, IMapper mapper)
        {
            _internService = internService;
            _logger = logger;
            _mapper = mapper;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _internService.GetByEmail(internRegister.Email);
                if (user != null)
                {
                    TempData["ErrorMessage"] = "Email has already existed.";
                    return Page();
                }

                Repositories.Entities.Intern intern = _mapper.Map<Repositories.Entities.Intern>(internRegister);
                intern.EmailVerifyCode = new Random().Next(100000, 1000000);
                intern.Status = 0;
                //{
                //    Ema = credential.Email,
                //    CustomerFullName = credential.Fullname,
                //    Telephone = credential.PhoneNumber,
                //    CustomerBirthday = credential.DateOfBirth,
                //    CustomerStatus = 0,
                //    Password = credential.Password,
                //    EmailVerifyCode = new Random().Next(100000, 1000000)
                //};

                //EmailSendModel emailSendModel = new()
                //{
                //    ToEmail = credential.Email,
                //    Body = intern.EmailVerifyCode.ToString(),
                //    IsBodyHtml = true,
                //    Subject = "Verify email"
                //};

                await _internService.RegisterIntern(intern);

                TempData["Email"] = intern.Email;

                //var emailSenderService = _serviceProvider.GetRequiredService<EmailSenderService>();
                //emailSenderService.EnqueueEmail(emailSendModel);
                //emailSenderService.ExecuteTask();

            }
            return RedirectToPage("/Common/VerifyMail");
            if (ModelState.IsValid)
            {
                if (await _internService.SignUp(internRegister))
                {
                    Message = "Register successfully!";
                    return RedirectToPage("/Index");
                }
                else
                {
                    Message = "Something went wrong!";
                }   
            }
            return Page();
        }
    }
}
