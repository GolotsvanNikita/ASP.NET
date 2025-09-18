using System.ComponentModel.DataAnnotations;

namespace GuestBookRazor.Models
{
    public class LoginModelView
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                 ErrorMessageResourceName = "NameField")]
        public string? Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                 ErrorMessageResourceName = "PassField")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}