using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPortal.Models;
using MusicPortal.Repositories;
using MusicPortal.Services;
using System.Threading.Tasks;

namespace MusicPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AccountController(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("password", "Password is required.");
            }
            else if (password.Length < 6)
            {
                ModelState.AddModelError("password", "Password must be at least 6 characters long.");
            }
            else if (password != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "Passwords do not match.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool hasAdmins = await _userRepository.GetAllUsersAsync().ContinueWith(t => t.Result.Any(u => u.IsAdmin));

                    user.PasswordHash = _passwordHasher.HashPassword(password);

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

                    await _userRepository.AddUserAsync(user);
                    return RedirectToAction("Login");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException?.Message.Contains("IX_Users_Login") == true)
                    {
                        ModelState.AddModelError("Login", "This login is already taken.");
                    }
                }
            }

            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Login and password are required.");
                return View();
            }

            string hash = _passwordHasher.HashPassword(password);
            var user = await _userRepository.GetUserByLoginAndPasswordAsync(login, hash);

            if (user != null && user.IsActive)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Login", user.Login);
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
                return RedirectToAction("Index", "Songs");
            }

            ModelState.AddModelError("", "Incorrect login or password or account is not active.");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}