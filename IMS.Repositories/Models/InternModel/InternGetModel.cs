using IMS.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Models.InternModel
{
    public class InternGetModel
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
        public string? WorkHistory { get; set; }
        public string? Skill { get; set; }
        public string? Education { get; set; }
        public bool? IsDelete { get; set; }

        public string assignment { get; set; }
        public List<string>? feedbacks { get; set; }
        public string? mentorName { get; set; }


    }
}
