namespace IMS.Models.Entities
{
    public class Document : BaseEntity
    {
        public string? Name { get; set; }
        public string? Link { get; set; }

        //Relationship
        public Guid? LessonId { get; set; }
        public virtual Lesson? Lesson { get; set; }
        
    }
}
