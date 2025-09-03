using Microsoft.EntityFrameworkCore;
using MusicPortal.DAL.EF;
using MusicPortal.DAL.Entities;

namespace MusicPortal.DAL.Repositories
{
    public class SongRepository : ISongRepository<Song>
    {
        private readonly MusicPortalContext _context;

        public SongRepository(MusicPortalContext context)
        {
            _context = context;
        }

        public async Task<List<Song>> GetAllSongs()
        {
            return await _context.Songs.Include(s => s.Genre).Include(s => s.User).ToListAsync();
        }

        public async Task<Song?> GetSongById(int id)
        {
            return await _context.Songs.FindAsync(id);
        }

        public async Task AddSong(Song song)
        {
            _context.Add(song);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSong(Song song)
        {
            _context.Update(song);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSong(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
        }
    }
}