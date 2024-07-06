using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Models.TrainingProgramModel
{
    public class TrainingProgramUpdateModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; } // Optional
        public DateTime? StartDate { get; set; } // Optional
        public DateTime? EndDate { get; set; }
    }
}
