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

        public async Task<List<Song>> GetAllSongsAsync()
        {
            return await _context.Songs.Include(s => s.Genre).Include(s => s.User).ToListAsync();
        }

        public async Task<Song?> GetSongByIdAsync(int id)
        {
            return await _context.Songs.FindAsync(id);
        }

        public async Task AddSongAsync(Song song)
        {
            _context.Add(song);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSongAsync(Song song)
        {
            _context.Update(song);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSongAsync(int id)
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