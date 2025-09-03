using Microsoft.EntityFrameworkCore;
using MusicPortal.DAL.EF;
using MusicPortal.DAL.Entities;

namespace MusicPortal.DAL.Repositories
{
    public class UserRepository : IUserRepository<User>
    {
        private readonly MusicPortalContext _context;

        public UserRepository(MusicPortalContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<User>> GetInactiveUsers()
        {
            return await _context.Users.Where(u => !u.IsActive).ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByLogin(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
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