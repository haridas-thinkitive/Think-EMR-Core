using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThinkEMR_Care.DataAccess.Models.Roles_and_Responsibility
{
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("RoleType")]
        public int RoleTypeId { get; set; }
        public RoleTypes RoleType { get; set; }

        [ForeignKey("Permission")]
        public int PermissionId { get; set; }

        public Permission Permission { get; set; }
    }
}
