using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPortal.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string? PasswordHash { get; set; }

        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
    }
}
