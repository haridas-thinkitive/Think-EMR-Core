using Role_And_Permission.Roles_and_Responsibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkEMR_Care.DataAccess.Models.Roles_and_Responsibility
{
    [Table("tblRolePermission")]

    public class RolePermission
    {
   
        [ForeignKey("RoleId")]
        public int RoleId { get; set; }
        public Role Role;

        [ForeignKey("PermissionId")]
        public int PermissionId { get; set; }
        public Permission Permission;


    }
}
