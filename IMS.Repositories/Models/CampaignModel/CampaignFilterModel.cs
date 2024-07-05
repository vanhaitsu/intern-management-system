

namespace IMS.Repositories.Models.CampaignModel
{
    public class CampaignFilterModel
    {
        public string Order { get; set; } = "creationDate";
        public bool OrderByDescending { get; set; } = true;
        public string? Search { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
