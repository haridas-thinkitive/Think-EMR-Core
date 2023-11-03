using System.ComponentModel.DataAnnotations;

namespace ThinkEMR_Care.Core.Models
{
    public class UserOTP
    {
        [Required(ErrorMessage = "OTP Requried Man..!!")]
        public string InputOTP { get; set; }
    }

}
