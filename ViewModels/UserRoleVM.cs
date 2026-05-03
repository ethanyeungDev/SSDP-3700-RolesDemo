using System.ComponentModel.DataAnnotations;

namespace RolesDemo.ViewModels
{
    public class UserRoleVM
    {
        [Display(Name = "ID")]
        public int? ID { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; } = string.Empty;
    }
}
