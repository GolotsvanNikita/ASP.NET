using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public  string PasswordHash { get; set; }

        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        public virtual ICollection<Song> Songs { get; set; }

    }
}
