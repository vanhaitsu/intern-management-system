namespace IMS.Models.Entities
{
    public class Campaign : BaseEntity
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }

        //Relationship
        public virtual ICollection<ProgramCampaign> ProgramCampaigns { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
