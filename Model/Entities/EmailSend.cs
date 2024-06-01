namespace IMS.Models.Entities
{
    public class EmailSend : BaseEntity
    {
        public DateTime? SendDate { get; set; }
        public string Content { get; set; }

        //Relationship
        public Guid? SenderId { get; set; }
        public virtual Account? Account { get; set; }

        public Guid? TemplateId { get; set; }
        public virtual EmailTemplate? EmailTemplate { get; set; }

        public virtual ICollection<EmailSendTrainee> EmailSendTrainee { get;}
    }
}
