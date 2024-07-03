namespace IMS.Models.Entities
{
    public class Assignment : BaseEntity
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }

        //Relationship
        public Guid? TrainingProgramId { get; set; }
        public virtual TrainingProgram? TrainingProgram { get; set; }
        
    }
}
