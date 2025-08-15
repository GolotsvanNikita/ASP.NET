using Films.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Films.Controllers
{
    public class FilmController : Controller
    {
        ApplicationDbContext _context;

        public FilmController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Film> films = await Task.Run(() => _context.Films);
            ViewBag.Films = films;
            return View();
        }
    }
}
