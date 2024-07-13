using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Models.NewFolder
{
    public class InterviewCreateModel
    {
        public Guid? InternId { get; set; }
        public string? InternName { get; set; }
        public string? InternEmail { get; set; }
        public DateTime? Date{ get; set; }
        public TimeOnly? Time { get; set; }
        public string? Location { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
