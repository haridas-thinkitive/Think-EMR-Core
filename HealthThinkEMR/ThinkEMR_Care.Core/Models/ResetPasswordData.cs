using System.ComponentModel.DataAnnotations;

namespace ThinkEMR_Care.Core.Models
{
    public class ResetPasswordData
    {
        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password And Conformation Password dosenot match")]
        public string ConfirmPassword { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Token { get; set; } = null!;

        public string OTP { get; set; } = null!;
    }
}
