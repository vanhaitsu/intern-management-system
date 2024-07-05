
using System.ComponentModel.DataAnnotations;

namespace IMS.Repositories.Models.CommonModel
{
    public class LoginModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string? Role { get; set; }

        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
