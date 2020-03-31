using System.ComponentModel.DataAnnotations;

namespace ShoppingWebApp.Models
{
    public class ResetPassword
    {
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 20 in length!")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers and underscores!")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 16 in length!")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Password can only contain letters, numbers and underscores!")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Password does not match above!")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ResetCode { get; set; }
    }
}