using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models.Entities
{
    public class EmailSendTrainee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        //Relationship
        public Guid? ReceiveId { get; set; }
        public virtual Trainee? Trainee { get; set; }

        public Guid? EmailSendId { get; set; }
        public virtual EmailSend? EmailSend { get; set; }
    }
}
