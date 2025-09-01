using MusicPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPortal.Repositories
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAllGenresAsync();
        Task<Genre?> GetGenreByIdAsync(int id);
        Task AddGenreAsync(Genre genre);
        Task UpdateGenreAsync(Genre genre);
        Task DeleteGenreAsync(int id);
    }
}