using GuestBookRazor.Models;

namespace GuestBookRazor.Repositories
{
    public interface IRepository
    {
        IList<Message> GetMessages();
        User? GetUserByName(string name);
        bool UserExists(string name);
        void AddUser(User user);
        void AddMessage(Message message);
        void SaveChanges();
    }
}
