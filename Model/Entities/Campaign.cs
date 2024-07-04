namespace IMS.Repositories.Entities
{
    public class Campaign : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; } // Optional
        public DateTime? EndDate { get; set; } // Optional

        //Relationship
        public virtual ICollection<Application>? Applications { get; set; } = new List<Application>();
    }
}
