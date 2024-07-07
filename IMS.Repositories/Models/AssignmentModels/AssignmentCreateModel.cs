using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Models.AssignmentModels
{
    public class AssignmentCreateModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be no more than 50 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; }
        public string? Material { get; set; }
        [Required(ErrorMessage = "Duration is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1.")]
        public int Duration { get; set; } // Optional
        [Required(ErrorMessage = "Start Date is required")]
        public DateTime? StartDate { get; set; } // Optional
        public DateTime? AssignedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? InternId { get; set; }
        public Guid? TrainingProgramId { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(256, ErrorMessage = "Email must be no more than 256 characters")]
        public string? InternEmail { get; set; }
        public string? KPI { get; set; }
    }
}
