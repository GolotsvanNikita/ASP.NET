using Films.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var film = _context.Films.FirstOrDefault(film => film.Id == id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Film film)
        {
            if (!ModelState.IsValid)
            {
                return View(film);
            }

            var updateFilm = _context.Films.FirstOrDefault(f => f.Id == film.Id);

            if(updateFilm == null)
            {
                return NotFound();
            }

            updateFilm.Name = film.Name;
            updateFilm.Director = film.Director;
            updateFilm.Genre = film.Genre;
            updateFilm.Year = film.Year;
            updateFilm.Description = film.Description;

            await _context.SaveChangesAsync();

            return await Task.Run(() => RedirectToAction("Index"));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var films = _context.Films.FirstOrDefault(film => film.Id == id);
            if (films != null)
            {
                _context.Films.Remove(films);
                await _context.SaveChangesAsync();
            }
            return await Task.Run(() => RedirectToAction("Index"));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Film film)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Films = await _context.Films.ToListAsync();
                return View("Index", film);
            }

            _context.Films.Add(film);
            await _context.SaveChangesAsync();
            return await Task.Run(() => RedirectToAction("Index"));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var film = await _context.Films.FirstOrDefaultAsync(f => f.Id == id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

    }
}
