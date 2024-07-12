using IMS.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Models.ApplicationModel
{
    public class ApplicationViewModel
    {
        public Guid ApplicationId { get; set; }
        public string? ApplicationName { get; set; }
        public string? Status { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid? InternId { get; set; }
        public Intern Intern { get; set; }
        public Campaign Campaign { get; set; }
        public DateTime ApplyDate { get; set; }
    }
}
