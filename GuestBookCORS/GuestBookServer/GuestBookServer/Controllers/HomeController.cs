using GuestBook.Models;
using GuestBook.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GuestBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;
        private readonly UserContext _userContext;

        public HomeController(IRepository repository, UserContext context)
        {
            _repository = repository;
            _userContext = context;
        }

        [HttpPost]
        public IActionResult AddMessage([FromBody] MessageInputModel input)
        {
            if (string.IsNullOrEmpty(input?.Message))
            {
                return BadRequest("Message is empty");
            }

            var name = HttpContext.Session.GetString("Name") ?? "Anonymous";

            var msg = new Message
            {
                MessageText = input.Message,
                Date = DateTime.Now
            };

            _repository.AddMessage(msg);
            _repository.SaveChanges();

            return Json(msg);
        }

        [HttpGet]
        public IActionResult GetMessage()
        {
            if (_userContext.Messages == null)
            {
                return Problem("Messages is null");
            }
            var messages = _repository.GetMessages();
            return Json(messages);
        }
    }
}