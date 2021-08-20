using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    public class UserAccount
    {
        [Key]
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }

    public class RegisterViewdel
    {
        [Required]
        public string Email { get; set; }

        // This will make it so the email has to match the orgi email
        [Compare(nameof(Email))]
        public string ConfirmEmail { get; set; }

        [Required]
        // This makes the text box a password format
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        // This will only show the date and not the time
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
    }
}
