using MusicPortal.DAL.Entities;

namespace MusicPortal.DAL.Repositories
{
    public interface ISongRepository<T> where T : Song
    {
        Task<List<Song>> GetAllSongs();
        Task<Song?> GetSongById(int id);
        Task AddSong(Song song);
        Task UpdateSong(Song song);
        Task DeleteSong(int id);
    }
}