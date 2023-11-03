using System.ComponentModel.DataAnnotations;

namespace ThinkEMR_Care.Core.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name Requried")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password Requried")]
        public string? Password { get; set; }
    }
}
