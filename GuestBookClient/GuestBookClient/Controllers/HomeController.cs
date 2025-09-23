using Microsoft.AspNetCore.Mvc;

namespace GuestBook.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}