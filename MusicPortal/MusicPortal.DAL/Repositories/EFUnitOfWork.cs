using MusicPortal.DAL.EF;
using MusicPortal.DAL.Entities;
using MusicPortal.DAL.Interfaces;

namespace MusicPortal.DAL.Repositories
{
    internal class EFUnitOfWork : IUnitOfWork
    {
        private MusicPortalContext db;
        private UserRepository userRepository;
        private GenreRepository genreRepository;
        private SongRepository songRepository;

        public EFUnitOfWork(MusicPortalContext context) 
        {
            this.db = context;
        }

        public IUserRepository<User> Users 
        {
            get 
            {
                if (userRepository == null) 
                {
                    userRepository = new UserRepository(db);
                }
                return userRepository;
            }
        }
        public IGenreRepository<Genre> Genres
        {
            get
            {
                if (genreRepository == null)
                {
                    genreRepository = new GenreRepository(db);
                }
                return genreRepository;
            }
        }
        public ISongRepository<Song> Songs
        {
            get
            {
                if (songRepository == null)
                {
                    songRepository = new SongRepository(db);
                }
                return songRepository;
            }
        }

        public async Task Save() 
        {
            await db.SaveChangesAsync();
        }
    }
}
