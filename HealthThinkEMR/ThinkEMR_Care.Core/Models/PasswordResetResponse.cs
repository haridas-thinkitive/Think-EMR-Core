namespace ThinkEMR_Care.Core.Models
{
    public class PasswordResetResponse
    {
        public bool Status { get; set; } 
        public string Message { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
}
