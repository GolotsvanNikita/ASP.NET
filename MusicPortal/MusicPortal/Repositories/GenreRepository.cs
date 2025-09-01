using Microsoft.EntityFrameworkCore;
using MusicPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPortal.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly MusicPortalContext _context;

        public GenreRepository(MusicPortalContext context)
        {
            _context = context;
        }

        public async Task<List<Genre>> GetAllGenresAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre?> GetGenreByIdAsync(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task AddGenreAsync(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGenreAsync(Genre genre)
        {
            _context.Update(genre);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGenreAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
            }
        }
    }
}