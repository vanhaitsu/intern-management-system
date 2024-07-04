using IMS.Repositories.Enums;

namespace IMS.Repositories.Entities
{
    public class Interview : BaseEntity
    {
        public string? Name { get; set; }
        public DateTime? DateTime { get; set; }
        public string? Location { get; set; }
        public InterviewStatus Status { get; set; }

        //Relationship
        public Guid? InternId { get; set; }
        public Intern? Intern { get; set; }
    }
}
