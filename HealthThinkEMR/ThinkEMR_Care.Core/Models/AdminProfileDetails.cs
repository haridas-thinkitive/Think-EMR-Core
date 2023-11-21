using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ThinkEMR_Care.Core.Models
{
    public class AdminProfileDetails
    {
            [Required(ErrorMessage = "User Name is Required")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Email is Required")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is Required")]
            public string Password { get; set; }

            [Required(ErrorMessage = "First Name is Required")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last Name is Required")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Contact Number is Required")]
            public string ContactNumber { get; set; }
            public DateTime? LastLogin { get; set; }
            public bool Status { get; set; }
            public bool IsDeleted { get; set; }
            public string? ProfileImage { get; set; }

            [Required(ErrorMessage = "Role is Required")]
            public string Role { get; set; }
    }
}
