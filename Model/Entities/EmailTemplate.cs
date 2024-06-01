namespace IMS.Models.Entities
{
    public class EmailTemplate : BaseEntity
    {
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
    
        //Relationship
        public virtual ICollection<EmailSend> EmailSends { get; set; }
    }
}
