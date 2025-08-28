using System.ComponentModel.DataAnnotations;

namespace GuestBook.Models
{
    public class RegisterModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string? PasswordConfirm { get; set; }
    }
}