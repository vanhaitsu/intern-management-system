using IMS.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Models.AssignmentModels
{
    public class AssignmentViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid InternId { get; set; }
        public Guid TrainingProgramId { get; set; }

        public Intern? intern { get; set; }

        public TrainingProgram? TrainingProgram { get; set; }
        
    }
}
