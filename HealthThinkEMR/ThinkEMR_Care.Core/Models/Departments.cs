using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkEMR_Care.Core.Models
{
    public class Departments
    {
        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        //public List<string> DepartmentAdmin { get; set; }
        public List<DepartmentAdmin> DepartmentAdmins { get; set; }
        public string ContactNumber { get; set; }
        public bool Status { get; set; }

    }

    public class DepartmentAdmin
    {
        [Key]
        public int DepartmentAdminId { get; set; }
        public string Name { get; set; }
    }
}
