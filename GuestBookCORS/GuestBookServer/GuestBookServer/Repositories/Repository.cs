using GuestBook.Models;
using Microsoft.EntityFrameworkCore;

namespace GuestBook.Repositories
{
    public class Repository : IRepository
    {
        private UserContext _context;

        public Repository(UserContext context)
        {
            _context = context;
        }

        public IEnumerable<Message> GetMessages()
        {
            return _context.Messages
                .OrderByDescending(m => m.Date)
                .ToList();
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
