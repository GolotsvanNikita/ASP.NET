using Microsoft.EntityFrameworkCore;
using MusicPortal.DAL.EF;
using MusicPortal.DAL.Entities;
using MusicPortal.DAL.Interfaces;

namespace MusicPortal.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private MusicPortalContext db;
        private UserRepository? userRepository;
        private GenreRepository? genreRepository;
        private SongRepository? songRepository;

        public EFUnitOfWork(MusicPortalContext context) 
        {
            db = context;
        }

        public IGenreRepository<Genre> Genres => genreRepository ??= new GenreRepository(db);
        public ISongRepository<Song> Songs => songRepository ??= new SongRepository(db);
        public IUserRepository<User> Users => userRepository ??= new UserRepository(db);

        public async Task Save()
        {
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
