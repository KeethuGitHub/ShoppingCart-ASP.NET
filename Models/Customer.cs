using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingWebApp.Models
{
    public class Customer
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 20 in length!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters!")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 20 in length!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters!")]
        public string LastName { get; set; }

        [Required]
        [StringLength(1, MinimumLength = 1)]
        public string Gender { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 20 in length!")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers and underscores!")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Password can only contain letters and numbers!")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]        
        [Display(Name = "Date of Birth")]
        public DateTime? Dateofbirth { get; set; }

        [Required]
        [Compare("UserAgreement", ErrorMessage = "You must accept the terms & conditions!")]
        public bool UserAgreement { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Join Date")]
        public DateTime JoinDate { get; set; }

        public string ResetCode { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}