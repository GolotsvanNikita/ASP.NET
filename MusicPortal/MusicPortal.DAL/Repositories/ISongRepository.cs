using MusicPortal.DAL.Entities;

namespace MusicPortal.DAL.Repositories
{
    public interface ISongRepository<T>
    {
        Task<List<Song>> GetAllSongsAsync();
        Task<Song?> GetSongByIdAsync(int id);
        Task AddSongAsync(Song song);
        Task UpdateSongAsync(Song song);
        Task DeleteSongAsync(int id);
    }
}