namespace IMS.Repositories.Entities
{
    public class Feedback : BaseEntity
    {
        // Note the creation date field of BaseEntity can be used
        public string? Comment {  get; set; }

        //Relationship
        public Guid InternId { get; set; }
        public Guid AccountId { get; set; } // This is mentor role
        public Guid TrainingProgramId { get; set; }
        public virtual Intern? Intern { get; set; }
        public virtual Account? Account { get; set; }
        public virtual TrainingProgram? TrainingProgram { get; set; }
    }
}
