using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models.Entities
{
    public class ProgramCampaign
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        //Relationship
        public Guid? ProgramId { get; set; }
        public virtual TrainingProgram? TrainingProgram { get; set; }

        public Guid? CampaignId {  get; set; }
        public virtual Campaign? Campaign { get; set; }


    }
}
