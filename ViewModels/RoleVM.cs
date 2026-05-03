using System.ComponentModel.DataAnnotations;

namespace RolesDemo.ViewModels
{
    public class RoleVM
    {
        [Display(Name = "ID")]
        public string? Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; } = string.Empty;
    }
}
