using MusicPortal.DAL.Entities;

namespace MusicPortal.DAL.Repositories
{
    public interface IGenreRepository<T> where T : Genre
    {
        Task<List<Genre>> GetAllGenres();
        Task<Genre?> GetGenreById(int id);
        Task AddGenre(Genre genre);
        Task UpdateGenre(Genre genre);
        Task DeleteGenre(int id);
    }
}