using MusicPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPortal.Repositories
{
    public interface IUserRepository
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