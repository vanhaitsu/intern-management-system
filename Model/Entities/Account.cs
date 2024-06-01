using System.ComponentModel.DataAnnotations;

namespace IMS.Models.Entities
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

        //Relationship
        public Guid? RoleId { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<TrainingProgram> TrainingPrograms { get; set; }
        public virtual ICollection<EmailSend> EmailSends { get; set; }
    }
}
