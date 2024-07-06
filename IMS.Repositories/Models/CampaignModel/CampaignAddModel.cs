
using System.ComponentModel.DataAnnotations;

namespace IMS.Repositories.Models.CampaignModel
{
    public class CampaignAddModel
    {
        [Required(ErrorMessage = "Campaign name is required")]
        [StringLength(50, ErrorMessage = "Campaign name must be no more than 50 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 50, ErrorMessage = "Description must be at least 50 chars and no more than 500 characters")]
        public string? Description { get; set; }

    }
}
