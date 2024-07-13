using IMS.Repositories.Enums;

namespace IMS.Repositories.Entities
{
    public class Interview : BaseEntity
    {
        public string? InternEmail { get; set; }
        public string? InternName { get; set; }
        public DateTime? Date { get; set; }
        public TimeOnly? Time { get; set; }
        public string? Location { get; set; }
        public InterviewStatus Status { get; set; }

        //Relationship
        public Guid? InternId { get; set; }
        public Intern? Intern { get; set; }
    }
}
