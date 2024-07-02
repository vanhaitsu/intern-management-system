using System;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.TraineeModel
{
    public class TraineeUpdateModel
    {
        [Required(ErrorMessage = "FullName is required")]
        [StringLength(50, ErrorMessage = "FullName must be no more than 50 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "ProgramId is required")]
        public Guid ProgramId { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required"), Phone(ErrorMessage = "Invalid phone format")]
        [StringLength(15, ErrorMessage = "PhoneNumber must be no more than 15 characters")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(256, ErrorMessage = "Email must be no more than 256 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(256, ErrorMessage = "Address must be no more than 256 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "University is required")]
        [StringLength(100, ErrorMessage = "University must be no more than 100 characters")]
        public string University { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [StringLength(50, ErrorMessage = "Code must be no more than 50 characters")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }
    }
}
