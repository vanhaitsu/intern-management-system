using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels.AccountModel
{
    public class AccountLoginModel
    {
        public Guid Id { get; set; }
        public string? Role { get; set; }
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
    }
}
