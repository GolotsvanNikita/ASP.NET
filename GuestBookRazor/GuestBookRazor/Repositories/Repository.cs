using GuestBookRazor.Models;
using Microsoft.EntityFrameworkCore;

namespace GuestBookRazor.Repositories
{
    public class Repository : IRepository
    {
        private UserContext _context;

        public Repository(UserContext context)
        {
            _context = context;
        }

        public IList<Message> GetMessages()
        {
            return _context.Messages
                .Include(m => m.User)
                .OrderByDescending(m => m.Date)
                .ToList();
        }

        public User? GetUserByName(string name)
        {
            return _context.Users.FirstOrDefault(u => u.Name == name);
        }

        public bool UserExists(string name)
        {
            return _context.Users.Any(u => u.Name == name);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
