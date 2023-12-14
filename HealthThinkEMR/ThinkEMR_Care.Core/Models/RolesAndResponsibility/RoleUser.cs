using System.ComponentModel.DataAnnotations;

namespace ThinkEMR_Care.DataAccess.Models.Roles_and_Responsibility
{
    public class RoleUser
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleType { get; set; }
        //public string Permissions { get; set; }
        public List<int> PermissionIds { get; set; } = new List<int>();

        public bool IsDeleted { get; set; }

    }
}
