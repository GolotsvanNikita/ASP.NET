using Microsoft.EntityFrameworkCore;

namespace MusicPortal.Models
{
    public class MusicPortalContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Song> Songs { get; set; }
        public MusicPortalContext(DbContextOptions<MusicPortalContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
