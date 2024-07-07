namespace IMS.Repositories.Models.TrainingProgramModel
{
    public class TrainingProgramGetModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; } // Optional
        public DateTime? StartDate { get; set; } // Optional
        public DateTime? EndDate { get; set; }
    }
}
