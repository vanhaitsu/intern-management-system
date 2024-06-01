namespace IMS.Models.Entities
{
    public class Assignment : BaseEntity
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Enđate { get; set; }
        public string Type { get; set; }

        //Relationship
        public virtual ICollection<TrainingProgram> TrainingPrograms { get; set; }
    }
}
