using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Models.FeedbackModel
{
    public class FeedbackCreateModel
    {
        public string commnent { get; set; }
        public Guid InternID { get; set; }  
        public Guid MentorId { get; set; }

        public Guid TraningProgramId { get; set; }

        public Guid CreatedBy { get; set; }
    }
}
