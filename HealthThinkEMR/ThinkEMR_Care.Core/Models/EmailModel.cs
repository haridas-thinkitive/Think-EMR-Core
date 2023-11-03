using System.ComponentModel.DataAnnotations;

namespace ThinkEMR_Care.Core.Models
{
    public class EmailModel
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
    }
}
