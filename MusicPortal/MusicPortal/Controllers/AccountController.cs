using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPortal.BLL.DTO;
using MusicPortal.BLL.Interfaces;
using MusicPortal.BLL.Services;

namespace MusicPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;

        public AccountController(IUserService userService, IPasswordHasher passwordHasher)
        {
            _userService = userService;   
            _passwordHasher = passwordHasher;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserDTO user, string password, string confirmPassword)
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
                    await _userService.RegisterUser(user, password);
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
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userService.AuthenticateUser(model.Login, model.Password);

            if (user != null && user.IsActive)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Login", user.Login);
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
                return RedirectToAction("Index", "Songs");
            }

            ModelState.AddModelError("", "Incorrect login or password or account is not active.");
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}