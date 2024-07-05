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

        public CampaignModel(ICampaignService campaignService)
        {
            _campaignService = campaignService;
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
                return Page();
            }
            
            if (await _campaignService.Update(id, updateModel))
            {
                Message = "Update successfully!";
                return RedirectToPage("./Campaign");
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
                
                if (await _campaignService.Create(newCampaign))
                {
                    Message = "Add successfully!";
                    return RedirectToPage("./Campaign");
                }
                else
                {
                    Message = "Something went wrong!";
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
                return Page();
            }

            var deleteResult = await _campaignService.Delete(id);
            if (!deleteResult)
            {
                Message = "Failed to delete campaign. Please try again.";
                return Page();
            }
            else
            {
                TempData["SuccessMessage"] = "Campaign deleted successfully.";
            }
            return RedirectToPage("./Campaign");
        }
        public async Task<IActionResult> OnPostActiveAsync(Guid id)
        {
            var campaignToActive = await _campaignService.GetCampaignAsync(id);
            if (campaignToActive == null)
            {
                Message = "Campaign not found.";
                return Page();
            }

            var activeResult = await _campaignService.Active(id);
            if (!activeResult)
            {
                Message = "Failed to active campaign. Please try again.";
                return Page();
            }
            else
            {
                TempData["SuccessMessage"] = "Campaign active successfully.";
            }
            return RedirectToPage("./Campaign");
        }

    }
}
