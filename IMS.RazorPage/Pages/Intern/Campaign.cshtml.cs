using IMS.Repositories.Models.ApplicationModel;
using IMS.Repositories.Models.CampaignModel;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace IMS.RazorPage.Pages.Intern
{
    public class CampaignModel : PageModel
    {
        private readonly ICampaignService _campaignService;

        [TempData]
        public string Message { set; get; }



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
            var intern = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            campaigns = await _campaignService.GetAllAvailableCampaigns(filterModel,intern);
            TotalCampaigns = await _campaignService.GetTotalCampaignsCount(filterModel);
            ViewData["Campaigns"] = campaigns;
            ViewData["TotalCampaignsCount"] = TotalCampaigns;
            return Page();
        }
        public async Task<IActionResult> OnPostApplyAsync(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RedirectToPage("/Admin/Account");
            }
            else
            {
                ApplicationAddModel addModel = new ApplicationAddModel
                {
                    CampaignId = id,
                    InternId = Guid.Parse(userId)
                };
                try
                {
                    if (await _campaignService.ApplyCampaign(addModel))
                    {
                        Message = "Apply successfully!";
                        TempData["SuccessMessage"] = Message;
                    }
                    else
                    {
                        Message = "You already apply for this campaign";
                        TempData["ErrorMessage"] = Message;
                    }
                }catch(Exception e)
                {
                    Message = "Something went wrong!";
                    TempData["ErrorMessage"] = Message;
                }
                return RedirectToPage("/Intern/Campaign");

            }
        }

    }
}
