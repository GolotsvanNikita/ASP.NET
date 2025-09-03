using MusicPortal.DAL.Entities;

namespace MusicPortal.DAL.Repositories
{
    public interface IUserRepository<T> where T : User
    {
        Task<List<User>> GetAllUsers();
        Task<List<User>> GetInactiveUsers();
        Task<User?> GetUserById(int id);
        Task<T?> GetUserByLogin(string login);
        Task AddUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(int id);
    }
}