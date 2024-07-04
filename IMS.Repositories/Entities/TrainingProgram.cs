namespace IMS.Repositories.Entities
{
    public class TrainingProgram : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; } // Optional
        public DateTime? StartDate { get; set; } // Optional
        public DateTime? EndDate { get; set; } // Optional

        //Relationship
        public virtual ICollection<Assignment>? Assignments { get; set; } = new List<Assignment>();
        public virtual ICollection<Feedback>? Feedbacks { get; set; } = new List<Feedback>();
    }
}
