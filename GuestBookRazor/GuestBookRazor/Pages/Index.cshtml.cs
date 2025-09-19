using GuestBookRazor.Models;
using GuestBookRazor.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuestBookRazor.Pages
{
    public class IndexModel : PageModel
    {

        private IRepository repo;

        public IndexModel(IRepository repository)
        {
            repo = repository;
        }

        public IList<Message> Message { get; set; } = default!;

        public void OnGet()
        {
            HttpContext.Session.SetString("path", Request.Path);
            Message = repo.GetMessages();
        }

        public IActionResult OnPost(string? message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return RedirectToAction("Index");
            }

            var name = HttpContext.Session.GetString("Name");
            if (name == null)
            {
                return RedirectToAction("Login");
            }

            var user = repo.GetUserByName(name);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var msg = new Message
            {
                UserId = user.Id,
                MessageText = message,
                Date = DateTime.Now
            };

            repo.AddMessage(msg);
            repo.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
