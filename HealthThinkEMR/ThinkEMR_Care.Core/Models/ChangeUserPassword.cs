namespace ThinkEMR_Care.Core.Models
{
    public class ChangeUserPassword
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConformNewPassword { get; set; }
    }
}
