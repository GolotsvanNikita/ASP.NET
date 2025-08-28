using Microsoft.AspNetCore.Mvc;
using GuestBook.Models;
using GuestBook.Repositories;
using GuestBook.Filters;

namespace GuestBook.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        private IRepository _repository;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var messages = _repository.GetMessages();
            HttpContext.Session.SetString("path", Request.Path);
            return View(messages);
        }

        [HttpPost]
        public IActionResult AddMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return RedirectToAction("Index");
            }

            var name = HttpContext.Session.GetString("Name");
            if (name == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _repository.GetUserByName(name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var msg = new Message
            {
                UserId = user.Id,
                MessageText = message,
                Date = DateTime.Now
            };

            _repository.AddMessage(msg);
            _repository.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}