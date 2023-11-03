using System.ComponentModel.DataAnnotations;

namespace ThinkEMR_Care.Core.Models
{
    public class Departments
    {
        [Key]
        public int Id { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentAdmin { get; set; }
        public string ContactNumber { get; set; }
        public bool Status { get; set; }

    }
}
