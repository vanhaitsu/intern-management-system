namespace IMS.Repositories.Entities
{
    public class Intern : BaseEntity
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
        public string? WorkHistory { get; set; }
        public string? Skill { get; set; }
        public string? Education { get; set; }

        //Relationship
        public virtual ICollection<Mentorship>? Mentorships { get; set; } = new List<Mentorship>();
        public virtual ICollection<Assignment>? Assignments { get; set; } = new List<Assignment>();
        public virtual ICollection<Application>? Applications { get; set; } = new List<Application>();
        public virtual ICollection<Feedback>? Feedbacks { get; set; } = new List<Feedback>();
    }
}
