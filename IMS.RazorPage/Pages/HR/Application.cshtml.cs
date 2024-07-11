
using AutoMapper;
using IMS.Repositories.Entities;
using IMS.Repositories.Enums;
using IMS.Repositories.Models.ApplicationModel;
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

        public List<ApplicationViewModel> Applications { get; set; }
        public List<Account> MentorAccount { get; set; }
        [BindProperty]
        public Guid ApplicationIdToUpdate { get; set; }
        [BindProperty]
        public string NewStatus { get; set; }
      
        public Guid CampaignId { get; set; }

        public Guid modifiedBy { get; set; }

        public ApplicationModel(IApplicationService applicationService, IMapper mapper, IAccountService accountService, IMentorshipService mentorshipService)
        {
            _applicationService = applicationService;
            _mapper = mapper;
            _accountService = accountService;
            _mentorshipService = mentorshipService;
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



    }
}
//=============================================================================================================================================================================================
