using System;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.TraineeModel
{
    public class TraineeLoginModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string University { get; set; }
        public string? Address { get; set; }
    }
}
