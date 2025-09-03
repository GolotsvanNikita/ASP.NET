using MusicPortal.BLL.DTO;

namespace MusicPortal.BLL.Interfaces
{
    public interface IGenreService
    {
        Task<List<GenreDTO>> GetAllGenres();
        Task<GenreDTO?> GetGenreById(int id);
        Task<bool> AddGenre(GenreDTO genre);
        Task<bool> UpdateGenre(GenreDTO genre);
        Task<bool> DeleteGenre(int id);
    }
}
