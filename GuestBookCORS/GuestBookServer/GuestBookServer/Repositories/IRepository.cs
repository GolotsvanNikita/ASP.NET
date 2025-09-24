using GuestBook.Models;

namespace GuestBook.Repositories
{
    public interface IRepository
    {
        IEnumerable<Message> GetMessages();
        void AddMessage(Message message);
        void SaveChanges();
    }
}
