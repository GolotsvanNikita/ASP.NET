using MusicPortal.DAL.Entities;
using MusicPortal.DAL.Repositories;

namespace MusicPortal.DAL.Interfaces
{
    internal interface IUnitOfWork
    {
        IUserRepository<User> Users { get; }
        IGenreRepository<Genre> Genres { get; }
        ISongRepository<Song> Songs { get; }
        Task Save();
    }
}
