using System.ComponentModel.DataAnnotations;

namespace ThinkEMR_Care.Core.Models
{
    public class ChangeUserPassword
    {
        [Required(ErrorMessage = "Please enter the current password.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please enter the new password.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm the new password.")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirm password do not match.")]
        public string ConformNewPassword { get; set; }
    }

}
