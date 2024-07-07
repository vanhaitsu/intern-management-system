using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Models.FeedbackModel
{
    public class FeedbackFilterModel
    {
        public string Order { get; set; } = "creationDate";
        public bool OrderByDescending { get; set; } = true;
        public Guid? CreatedBy { get; set; }
        public Guid? InternId { get; set; }
        public Guid? TrainingProgramId { get; set; }
        public Guid? AccountId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? Search { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
