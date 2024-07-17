using IMS.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Models.InterviewModel
{
    public class InterviewGetModel
    {
        public Guid Id { get; set; }
        public string? InternMail { get; set; }
        public string? InternName { get; set; }
        public DateTime? Date { get; set; }
        public TimeOnly? Time { get; set; }
        public string? Location { get; set; }
        public InterviewStatus Status { get; set; }
    }
}
