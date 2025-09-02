using System.ComponentModel.DataAnnotations;

namespace MusicPortal.DAL.Entities
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Song>? Songs { get; set; }
    }
}
