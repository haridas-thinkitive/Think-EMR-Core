using System.ComponentModel.DataAnnotations;

namespace ThinkEMR_Care.Core.Models
{
    public class Departments
    {
        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentAdmin { get; set; }
        
        //public List<DepartmentAdmin> DepartmentAdmins { get; set; }
        public string ContactNumber { get; set; }
        public bool Status { get; set; }

    }
}
