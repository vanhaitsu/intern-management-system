using IMS.Repositories.Enums;

namespace IMS.Repositories.Entities
{
    public class Application : BaseEntity
    {
        public DateTime? AppliedDate { get; set; }
        public ApplicationStatus? Status { get; set; }

        //Relationship
        public Guid? InternId { get; set; }
        public Guid? CampaignId { get; set; }
        public Intern? Intern { get; set; }
        public Campaign Campaign { get; set; }
    }
}
