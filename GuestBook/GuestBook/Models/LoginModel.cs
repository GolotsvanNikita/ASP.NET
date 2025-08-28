using System.ComponentModel.DataAnnotations;

namespace GuestBook.Models
{
    public class LoginModel
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