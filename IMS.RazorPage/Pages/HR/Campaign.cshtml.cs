using IMS.Repositories.Entities;
using IMS.Repositories.Models.CampaignModel;
using IMS.Repositories.Models.InternModel;
using IMS.Services.Interfaces;
using IMS_View.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.RazorPage.Pages.HR
{
    public class CampaignModel : PageModel
    {
        private readonly ICampaignService _campaignService;
        private readonly IApplicationService _applicationService;

        public string Message { set; get; }

        [BindProperty]
        public CampaignAddModel campaign { set; get; }
        [BindProperty]
        public CampaignAddModel newCampaign { set; get; }

        [BindProperty]
        public CampaignUpdateModel updateCampaign { set; get; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public int TotalCampaigns { get; set; }
        public List<CampaignGetModel> campaigns { get; set; }

        [BindProperty(SupportsGet = true)]
        public CampaignFilterModel filterModel { get; set; } = new CampaignFilterModel();

        public CampaignModel(ICampaignService campaignService, IApplicationService applicationService)
        {
            _campaignService = campaignService;
            _applicationService = applicationService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            filterModel.PageSize = PageSize;
            filterModel.PageNumber = PageNumber;
            filterModel.Search = SearchTerm;
            campaigns = await _campaignService.GetAllCampaigns(filterModel);
            TotalCampaigns = await _campaignService.GetTotalCampaignsCount(filterModel);
            ViewData["Campaigns"] = campaigns;
            ViewData["TotalCampaignsCount"] = TotalCampaigns;
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync(Guid id)
        {
            CampaignUpdateModel updateModel = updateCampaign;
            var existedCampaign = await _campaignService.GetCampaignAsync(id);
            if (existedCampaign == null)
            {
                Message = "Campaign not found.";
                TempData["ErrorMessage"] = Message;
                return Page();
            }

            if (await _campaignService.Update(id, updateModel))
            {
                Message = "Update successfully!";
                TempData["SuccessMessage"] = Message;
                return RedirectToPage("/HR/Campaign");
            }
            else
            {
                Message = "Failed to update!";
                TempData["ErrorMessage"] = Message;
                return Page(); 
            }
        }
        public async Task<IActionResult> OnPostAddAsync()
        {
            if (newCampaign.Name == null || newCampaign.Description == null)
            {
                Message = "Information cannot empty!";
                TempData["ErrorMessage"] = Message;
                return RedirectToPage("/HR/Campaign");
            }

            if (!ModelState.IsValid)
            {
                
                if (await _campaignService.Create(newCampaign))
                {
                    Message = "Add successfully!";

                    TempData["SuccessMessage"] = Message;
                    return RedirectToPage("/HR/Campaign");
                }
                else
                {
                    Message = "Something went wrong!";
                    TempData["ErrorMessage"] = Message;
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUnactiveAsync(Guid id)
        {
            var campaignToDelete = await _campaignService.GetCampaignAsync(id);
            if (campaignToDelete == null)
            {
                Message = "Campaign not found.";
                TempData["ErrorMessage"] = Message;
                return Page();
            }

            var deleteResult = await _campaignService.Delete(id);
            if (!deleteResult)
            {
                Message = "Failed to delete campaign. Please try again.";
                TempData["ErrorMessage"] = Message;

                return Page();
            }
            else
            {
                TempData["SuccessMessage"] = "Campaign deleted successfully.";

            }
            return RedirectToPage("/HR/Campaign");
        }
        public async Task<IActionResult> OnPostActiveAsync(Guid id)
        {
            var campaignToActive = await _campaignService.GetCampaignAsync(id);
            if (campaignToActive == null)
            {
                Message = "Campaign not found.";
                TempData["ErrorMessage"] = Message;
                return Page();
            }

            var activeResult = await _campaignService.Active(id);
            if (!activeResult)
            {
                Message = "Failed to active campaign. Please try again.";
                TempData["ErrorMessage"] = Message;
                return Page();
            }
            else
            {
                TempData["SuccessMessage"] = "Campaign active successfully.";
            }
            return RedirectToPage("/HR/Campaign");
        }
        public IActionResult OnGetViewApplications(Guid campaignId)
        {
           

            ViewData["Applications"] = _applicationService.GetApplicationsByCampaignID(campaignId);

            return RedirectToPage("/HR/Application", new { campaignId });
        }


    }
}
