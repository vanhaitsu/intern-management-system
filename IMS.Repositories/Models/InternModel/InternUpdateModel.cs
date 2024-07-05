using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Models.InternModel
{
    public class InternUpdateModel
    {
        [Required(ErrorMessage = "FullName is required")]
        [StringLength(50, ErrorMessage = "FullName must be no more than 50 characters")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(256, ErrorMessage = "Email must be no more than 256 characters")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required"), Phone(ErrorMessage = "Invalid phone format")]
        [StringLength(15, ErrorMessage = "PhoneNumber must be no more than 15 characters")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(256, ErrorMessage = "Address must be no more than 256 characters")]
        public string? Address { get; set; }
        public string? WorkHistory { get; set; }

        [Required(ErrorMessage = "Skill is required")]
        [StringLength(256, ErrorMessage = "Skill must be no more than 256 characters")]
        public string? Skill { get; set; }

        [Required(ErrorMessage = "Education is required")]
        [StringLength(256, ErrorMessage = "Education must be no more than 256 characters")]
        public string? Education { get; set; }
    }
}
