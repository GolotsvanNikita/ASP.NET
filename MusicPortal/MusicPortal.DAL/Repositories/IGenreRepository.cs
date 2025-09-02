using MusicPortal.DAL.Entities;

namespace MusicPortal.DAL.Repositories
{
    public interface IGenreRepository<T>
    {
        Task<List<Genre>> GetAllGenresAsync();
        Task<Genre?> GetGenreByIdAsync(int id);
        Task AddGenreAsync(Genre genre);
        Task UpdateGenreAsync(Genre genre);
        Task DeleteGenreAsync(int id);
    }
}