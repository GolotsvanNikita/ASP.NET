using GuestBook.Models;

namespace GuestBook.Repositories
{
    public interface IRepository
    {
        IEnumerable<Message> GetMessages();
        User? GetUserByName(string name);
        bool UserExists(string name);
        void AddUser(User user);
        void AddMessage(Message message);
        void SaveChanges();
    }
}
