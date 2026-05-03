using System.ComponentModel.DataAnnotations;

namespace RolesDemo.ViewModels
{
    public class UserVM
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }
}
