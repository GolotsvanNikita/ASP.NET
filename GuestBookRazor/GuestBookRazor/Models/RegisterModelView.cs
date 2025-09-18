using System.ComponentModel.DataAnnotations;

namespace GuestBookRazor.Models
{
    public class RegisterModelView
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                 ErrorMessageResourceName = "NameField")]
        public string? Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                 ErrorMessageResourceName = "PassField")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                 ErrorMessageResourceName = "PassConfField")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Resource),
                 ErrorMessageResourceName = "PassMatch")]
        [DataType(DataType.Password)]
        public string? PasswordConfirm { get; set; }
    }
}