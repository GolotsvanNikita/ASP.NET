using Microsoft.EntityFrameworkCore;
using MusicPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPortal.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MusicPortalContext _context;

        public UserRepository(MusicPortalContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<User>> GetInactiveUsersAsync()
        {
            return await _context.Users.Where(u => !u.IsActive).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByLoginAndPasswordAsync(string login, string passwordHash)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.PasswordHash == passwordHash);
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}