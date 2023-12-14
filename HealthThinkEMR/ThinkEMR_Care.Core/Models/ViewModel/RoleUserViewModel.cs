using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ThinkEMR_Care.DataAccess.Models.Roles_and_Responsibility;

namespace ThinkEMR_Care.Core.Models.ViewModel
{
    public class RoleUserViewModel
    {
        public SelectList RoleTypes { get; set; }
        public List<Permission> Permissions { get; set; }
        public RoleUser RoleUser { get; set; }
        public List<int> SelectedPermissions { get; set; } // To store selected permission IDs


    }

    public class RoleUserVM
    {
        public string RoleName {  get; set; }
        public int RoleType { get; set; }
        public List<int> SelectedPermissions { get; set; }
    }
}
