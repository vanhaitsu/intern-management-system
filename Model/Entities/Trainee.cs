using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models.Entities
{
    public class Trainee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string? Gender { get; set; }
        public string? University { get; set; }
        public string? Code { get; set; }
        public string? Status { get; set; }

        //Relationship
        public Guid? ProgramId { get; set; }
        public virtual TrainingProgram? TrainingProgram { get; set; }

        public virtual ICollection<Score> Scores { get; set; }

        public virtual ICollection<EmailSendTrainee> EmailSendTrainees { get; set; }
    }
}
