using Azure;
using GuestBook.Models;
using GuestBook.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GuestBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;
        private UserContext _userContext; 

        public HomeController(IRepository repository, UserContext context)
        {
            _repository = repository;
            _userContext = context;
        }

        //public IActionResult Index()
        //{
        //    var messages = _repository.GetMessages();
        //    return View(messages);
        //}

        [HttpPost]
        public IActionResult AddMessage([FromBody] MessageInputModel input)
        {
            if (string.IsNullOrEmpty(input?.Message))
            {
                return Problem("Message is none");
            }

            var name = HttpContext.Session.GetString("Name");
            if (name == null)
            {
                return Problem("Name is null");
            }

            var msg = new Message
            {
                MessageText = input.Message,
                Date = DateTime.Now
            };

            _repository.SaveChanges();

            string response = JsonConvert.SerializeObject(msg);
            return Json(response);
        }

        public IActionResult GetMessage() 
        {
            if (_userContext.Messages == null)
            {
                return Problem("Messages is null");
            }
            var messages = _repository.GetMessages();
            string response = JsonConvert.SerializeObject(messages);
            return Json(response);
        }
    }
}