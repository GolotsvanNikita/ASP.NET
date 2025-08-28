using GuestBook.Filters;
using GuestBook.Models;
using GuestBook.Repositories;
using GuestBook.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GuestBook.Controllers
{
    [Culture]
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
            HttpContext.Session.SetString("path", Request.Path);
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
                    ModelState.AddModelError("", Resources.Resource.WrongPassOrLog);
                    HttpContext.Session.SetString("path", Request.Path);
                    return View(login);
                }
                HttpContext.Session.SetString("Name", user.Name);
                return RedirectToAction("Index", "Home");
            }
            HttpContext.Session.SetString("path", Request.Path);
            return View(login);
        }

        public IActionResult Register()
        {
            HttpContext.Session.SetString("path", Request.Path);
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
                    ModelState.AddModelError("", Resources.Resource.AlreadyExists);
                    HttpContext.Session.SetString("path", Request.Path);
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
            HttpContext.Session.SetString("path", Request.Path);
            return View(reg);
        }

        public ActionResult ChangeCulture(string lang)
        {
            string? returnUrl = HttpContext.Session.GetString("path");

            List<string> cultures = new List<string>() { "en", "uk" };
            if (!cultures.Contains(lang))
            {
                lang = "en";
            }

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(10);
            Response.Cookies.Append("lang", lang, option);
            return Redirect(returnUrl);
        }
    }
}