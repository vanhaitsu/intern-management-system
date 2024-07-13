
using AutoMapper;
using IMS.Repositories.Entities;
using IMS.Repositories.Enums;
using IMS.Repositories.Models.ApplicationModel;
using IMS.Repositories.Models.NewFolder;
using IMS.Repositories.Models.TrainingProgramModel;
using IMS.Services.Interfaces;
using IMS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Security.Claims;

namespace IMS.RazorPage.Pages.HR
{
    public class ApplicationModel : PageModel
    {
        private readonly IApplicationService _applicationService;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IMentorshipService _mentorshipService;
        private readonly IEmailService _emailService;
        private readonly IInterviewService _interviewService;
        private readonly IInternService _internService;

        public List<ApplicationViewModel> Applications { get; set; }
        public List<Account> MentorAccount { get; set; }
        [BindProperty]
        public Guid ApplicationIdToUpdate { get; set; }
        [BindProperty]
        public string NewStatus { get; set; }

        [BindProperty]
        public InterviewCreateModel InterviewCreateModel { get; set; }

        public Guid CampaignId { get; set; }

        public Guid modifiedBy { get; set; }

        public ApplicationModel(IApplicationService applicationService, IMapper mapper, IAccountService accountService, 
            IMentorshipService mentorshipService, IInterviewService interviewService, IEmailService emailService,
            IInternService internService)
        {
            _applicationService = applicationService;
            _mapper = mapper;
            _accountService = accountService;
            _mentorshipService = mentorshipService;
            _interviewService = interviewService;
            _emailService = emailService;
            _internService = internService;
        }

        public async Task<IActionResult> OnGetAsync(Guid campaignId, Guid applicationId)
        {
            MentorAccount = await _accountService.GetMentorAccount();
            CampaignId = campaignId;
            var appList = await _applicationService.GetApplicationsByCampaignID(campaignId);

            Applications = _mapper.Map<List<ApplicationViewModel>>(appList);
            return Page();
        }
        public async Task<IActionResult> OnPostUpdateStatusAsync()
        {
            modifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (ApplicationIdToUpdate == Guid.Empty || string.IsNullOrEmpty(NewStatus))
            {
                TempData["ErrorMessage"] = "Invalid application ID or status.";
                return RedirectToPage("./Application", CampaignId);
            }

            var updateResult = await _applicationService.UpdateStatus(ApplicationIdToUpdate, NewStatus, modifiedBy);

            if (updateResult)
            {
                TempData["SuccessMessage"] = "Application status updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update application status.";
            }

            return RedirectToPage("./Application", CampaignId);
        }

        public async Task<IActionResult> OnPostLinkMentor(Guid internId, Guid mentorId, Guid campaignId)
        {
            try
            {
                var createdBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var result = await _mentorshipService.LinkMentorforIntern(internId, mentorId, createdBy);
                if (result)
                {

                    var AppliToUpdate = await _applicationService.GetByInternIdAndCampaignId(internId, campaignId);
                    if (AppliToUpdate != null) {
                        AppliToUpdate.Status = ApplicationStatus.Accpeted;
                        AppliToUpdate.CreatedBy = createdBy;
                        await _applicationService.UpdateStatus(AppliToUpdate.Id, AppliToUpdate.Status.ToString(), createdBy);
                    }
                    TempData["SuccessMessage"] = "Mentor linked to intern successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to link mentor to intern.";
                }


            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error to link mentor to intern.";
                Console.WriteLine(ex.ToString());
            }
            return RedirectToPage("./Application", CampaignId);
        }

        public async Task<IActionResult> OnPostInterview(Guid internId)
        {
            try
            {
                var intern = await _internService.GetByEmail(InterviewCreateModel.InternEmail);
                if (intern == null)
                {
                    TempData["ErrorMessage"] = "Intern not found!";
                    return RedirectToPage("./Application");
                }
                InterviewCreateModel.InternId = intern.Id;
                InterviewCreateModel.Body =
                    $@"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Interview Invitation</title>
                    </head>
                    <body>
                        <p>Dear {InterviewCreateModel.InternName},</p>

                        <p>I hope this email finds you well.</p>

                        <p>We are pleased to inform you that you have been shortlisted for the Intern position at ABC. After reviewing your application and resume, we believe that your skills and experiences make you a strong candidate for this role. We would like to invite you to an interview to further discuss how your background, skills, and interests align with the needs of our team. Below are the details for the interview:</p>

                        <ul>
                            <li>Date: {InterviewCreateModel.Date.Value.ToShortDateString()}</li>
                            <li>Time: {InterviewCreateModel.Time.Value.ToShortTimeString()}</li>
                            <li>Location: {InterviewCreateModel.Location}</li>
                        </ul>

                        <p>Please confirm your availability for the proposed time. If you have any scheduling conflicts, feel free to suggest alternative dates and times that may work for you. Additionally, if you have any questions prior to the interview, please do not hesitate to reach out. We look forward to the opportunity to learn more about you and discuss how you can contribute to FAMSOJT Company.</p>

                        <p>Thank you for your interest in joining our team.</p>

                        <p>Best regards,<br>FAMSOJT Company</p>
                    </body>
                    </html>
                    ";
                
                _emailService.SendEmailAsync(InterviewCreateModel.InternEmail, InterviewCreateModel.Subject, InterviewCreateModel.Body, true);

                if (await _interviewService.Create(InterviewCreateModel))
                {
                    TempData["SuccessMessage"] = "Send mail successfully!";
                    return RedirectToPage("./Application");
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong!";
                Console.WriteLine(ex.ToString());
            }
            return RedirectToPage("./Application", CampaignId);
        }

    }
}
//=============================================================================================================================================================================================
