using Microsoft.AspNetCore.Mvc;
using MusicPortal.Models;
using MusicPortal.Repositories;
using System.Threading.Tasks;

namespace MusicPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AdminController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "True";
        }

        public async Task<IActionResult> Users()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            var users = await _userRepository.GetAllUsersAsync();
            return View(users);
        }

        public async Task<IActionResult> RegistrationRequests()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            var requests = await _userRepository.GetInactiveUsersAsync();
            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            await _userRepository.DeleteUserAsync(id);
            return RedirectToAction("RegistrationRequests");
        }

        [HttpPost]
        public async Task<IActionResult> ActivateUser(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                user.IsActive = true;
                await _userRepository.UpdateUserAsync(user);
            }
            return RedirectToAction("RegistrationRequests");
        }

        [HttpPost]
        public async Task<IActionResult> MakeAdmin(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                user.IsAdmin = true;
                await _userRepository.UpdateUserAsync(user);
            }
            return RedirectToAction("Users");
        }
    }
}