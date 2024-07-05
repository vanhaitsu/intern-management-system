using System.ComponentModel.DataAnnotations;

namespace IMS.Repositories.Entities
{
    public class Assignment : BaseEntity
    {
        public string Name {  get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? Comment { get; set; } // This can be used as Feedback from Mentor
        public string? KPI { get; set; }
        public string? Material { get; set; }
        public int? Duration { get; set; } // Optional
        public DateTime? StartDate { get; set; } // Optional
        public DateTime? EndDate { get; set; } // Optional
        public DateTime? AssignedDate { get; set; }
        [Range(0, 100, ErrorMessage = "Performance rating must be between 0 and 100.")]
        public double? PerformanceRating { get; set; }


        //Relationship
        public Guid? InternId { get; set; }
        public Guid TrainingProgramId { get; set; }
        public virtual Intern? Intern { get; set; }
        public virtual TrainingProgram? TrainingProgram { get; set; }
    }
}
