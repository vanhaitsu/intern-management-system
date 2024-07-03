using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.TrainingProgramModel
{
    public class TrainingProgramCreateModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Duration { get; set; }
        public string? Password { get; set; }
        public string Status { get; set; }

        public Guid? AccountId { get; set; }
    }
}
