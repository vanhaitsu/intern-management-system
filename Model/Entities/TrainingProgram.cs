using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models.Entities
{
    public class TrainingProgram
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Duration { get; set; }
        public string Status { get; set; }

        //Relationship
        public Guid? AccountId { get; set; }
        public virtual Account? Account { get; set; }

        public Guid? AssignmentId { get; set; }
        public virtual Assignment? Assignment { get; set;}

        public virtual ICollection<Trainee> Trainees { get; set; }

        public virtual ICollection<ProgramCampaign> ProgramCampaigns { get; set; }  
    }
}
