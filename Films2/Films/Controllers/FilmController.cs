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
        IWebHostEnvironment _appEnvironment;

        public FilmController(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
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
        public async Task<IActionResult> Edit(Film film, IFormFile uploded)
        {
            ModelState.Remove("uploded");

            if (!ModelState.IsValid)
            {
                return View(film);
            }

            var updateFilm = _context.Films.FirstOrDefault(f => f.Id == film.Id);

            if (updateFilm == null)
            {
                return NotFound();
            }

            updateFilm.Name = film.Name;
            updateFilm.Director = film.Director;
            updateFilm.Genre = film.Genre;
            updateFilm.Year = film.Year;
            updateFilm.Description = film.Description;

            if (uploded != null)
            {
                string path = "/images/" + uploded.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploded.CopyToAsync(fileStream);
                }
                updateFilm.PosterPath = "images/" + uploded.FileName;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
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
        public async Task<IActionResult> Create(Film film, IFormFile uploded)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Films = await _context.Films.ToListAsync();
                return View("Index", film);
            }

            if (uploded != null)
            {
                string path = "/images/" + uploded.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploded.CopyToAsync(fileStream);
                }
                film.PosterPath = "images/" + uploded.FileName;
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
