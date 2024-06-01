namespace IMS.Models.Entities
{
    public class Lesson : BaseEntity
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }

        //Relationship
        public Guid? CampaignId { get; set; }
        public virtual Campaign? Campaign { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

    }
}
