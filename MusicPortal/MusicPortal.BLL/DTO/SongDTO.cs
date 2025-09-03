using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPortal.BLL.DTO
{
    public class SongDTO
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? FilePath { get; set; }

        public int UserId { get; set; }
        public UserDTO? User { get; set; }
        public int GenreId { get; set; }
        public GenreDTO? Genre { get; set; }
    }
}
