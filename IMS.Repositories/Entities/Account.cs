namespace IMS.Repositories.Entities
{
    public class Account : BaseEntity
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }

        //Relationship
        public Guid? RoleId { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<Mentorship> Mentorships { get; set; } = new List<Mentorship>();
        public virtual ICollection<Feedback>? Feedbacks { get; set; } = new List<Feedback>();
    }
}
