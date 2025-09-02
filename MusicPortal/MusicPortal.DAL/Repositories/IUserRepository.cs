using MusicPortal.DAL.Entities;

namespace MusicPortal.DAL.Repositories
{
    public interface IUserRepository<T>
    {
        Task<List<User>> GetAllUsersAsync();
        Task<List<User>> GetInactiveUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByLoginAndPasswordAsync(string login, string passwordHash);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}