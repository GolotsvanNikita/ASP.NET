using MusicPortal.BLL.DTO;
using MusicPortal.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPortal.BLL.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsers();
        Task<List<UserDTO>> GetInactiveUsers();
        Task<UserDTO?> GetUserById(int id);
        Task<UserDTO?> AuthenticateUser(string login, string password);
        Task<bool> RegisterUser(UserDTO user, string password);
        Task<bool> UpdateUser(UserDTO user);
        Task<bool> DeleteUser(int id);
        Task<bool> ActivateUser(int id);
        Task<bool> MakeAdmin(int id);
        Task<bool> HasAdmins();
    }
}
