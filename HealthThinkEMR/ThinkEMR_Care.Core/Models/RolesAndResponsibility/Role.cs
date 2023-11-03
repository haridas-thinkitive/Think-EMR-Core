using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ThinkEMR_Care.DataAccess.Models.Roles_and_Responsibility;

namespace Role_And_Permission.Roles_and_Responsibility
{
    [Table("tblRole")]

    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        [ForeignKey("RoleTypeId")]
        public int RoleTypeId { get; set; }
        public RoleType RoleType;
    }
}
