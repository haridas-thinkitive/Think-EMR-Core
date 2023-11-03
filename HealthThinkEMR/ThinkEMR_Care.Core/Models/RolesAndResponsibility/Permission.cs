using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkEMR_Care.DataAccess.Models.Roles_and_Responsibility
{
    [Table("tblPermission")]
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }

        [ForeignKey("RoleTypeId")]
        public int RoleTypeId { get; set; }
        public RoleType RoleType;
        public string PermissionName { get; set; }
        public string PermissionDescription { get; set; }
        public bool PermissionStatus { get; set; }


    }
}
