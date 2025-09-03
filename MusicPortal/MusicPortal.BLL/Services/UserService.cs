using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicPortal.BLL.DTO;
using MusicPortal.BLL.Interfaces;
using MusicPortal.DAL.Interfaces;
using MusicPortal.DAL.Entities;

namespace MusicPortal.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAllUsers();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<List<UserDTO>> GetInactiveUsers()
        {
            var users = await _unitOfWork.Users.GetInactiveUsers();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO?> GetUserById(int id)
        {
            var user = await _unitOfWork.Users.GetUserById(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO?> AuthenticateUser(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return null;

            var user = await _unitOfWork.Users.GetUserByLogin(login);
            if (user == null || !_passwordHasher.VerifyPassword(password, user.PasswordHash))
                return null;

            return user.IsActive ? _mapper.Map<UserDTO>(user) : null;
        }

        public async Task<bool> RegisterUser(UserDTO userDto, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(password) || password.Length < 6) 
                {
                    return false;
                }

                var user = _mapper.Map<User>(userDto);
                user.PasswordHash = _passwordHasher.HashPassword(password);

                bool hasAdmins = await HasAdmins();

                if (!hasAdmins)
                {
                    user.IsAdmin = true;
                    user.IsActive = true;
                }
                else
                {
                    user.IsAdmin = false;
                    user.IsActive = false;
                }

                await _unitOfWork.Users.AddUser(user);
                await _unitOfWork.Save();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUser(UserDTO userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                await _unitOfWork.Users.UpdateUser(user);
                await _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                await _unitOfWork.Users.DeleteUser(id);
                await _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActivateUser(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserById(id);
                if (user == null)
                {
                    return false;
                }

                user.IsActive = true;
                await _unitOfWork.Users.UpdateUser(user);
                await _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> MakeAdmin(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserById(id);
                if (user == null)
                {
                    return false;
                }

                user.IsAdmin = true;
                await _unitOfWork.Users.UpdateUser(user);
                await _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> HasAdmins()
        {
            var users = await _unitOfWork.Users.GetAllUsers();
            return users.Any(u => u.IsAdmin);
        }
    }
}