using GuestBook.Models;
using GuestBook.Repositories;
using GuestBook.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GuestBook.Controllers
{
    public class AccountController : Controller
    {
        private IRepository _repository;
        private IPasswordHash _hasher;

        public AccountController(IRepository repository, IPasswordHash hasher)
        {
            _repository = repository;
            _hasher = hasher;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var user = _repository.GetUserByName(login.Name);
                if (user == null || user.Password != _hasher.Hash(login.Password))
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(login);
                }
                HttpContext.Session.SetString("Name", user.Name);
                return RedirectToAction("Index", "Home");
            }
            return View(login);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel reg)
        {
            if (ModelState.IsValid)
            {
                if (_repository.UserExists(reg.Name))
                {
                    ModelState.AddModelError("", "User with this login already exists!");
                    return View(reg);
                }

                User user = new User
                {
                    Name = reg.Name,
                    Password = _hasher.Hash(reg.Password)
                };
                _repository.AddUser(user);
                _repository.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(reg);
        }
    }
}