using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkEMR_Care.DataAccess.Models.Roles_and_Responsibility
{
    [Table("tblRoleType")]
    public class RoleType
    {
        [Key]
        public int RoleTypeId { get; set; }
        public string RoleTypeName { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
