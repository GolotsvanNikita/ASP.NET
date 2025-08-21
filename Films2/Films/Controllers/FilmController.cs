using Films.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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
        public async Task<IActionResult> Edit(Film film, IFormFile uploaded)
        {
            ModelState.Remove("uploaded");

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

            if (uploaded != null)
            {
                string path = "/images/" + uploaded.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploaded.CopyToAsync(fileStream);
                }
                updateFilm.PosterPath = "images/" + uploaded.FileName;
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
        public async Task<IActionResult> Create([Bind("Id,Name,Director,Year,Genre,Description")] Film film, IFormFile uploaded)
        {
            ModelState.Remove("uploaded");

            if (!ModelState.IsValid)
            {
                ViewBag.Films = await _context.Films.ToListAsync();
                return View("Index", film);
            }

            bool exists = await _context.Films.AnyAsync(f =>
            (f.Name != null && film.Name != null && f.Name.ToLower() == film.Name.ToLower()) &&
            (f.Director != null && film.Director != null && f.Director.ToLower() == film.Director.ToLower()) &&
            f.Year == film.Year);

            if (exists)
            {
                ModelState.AddModelError("", "This film already exists");
                ViewBag.Films = await _context.Films.ToListAsync();
                return View("Index", film);
            }

            if (uploaded != null)
            {
                string path = "/images/" + uploaded.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploaded.CopyToAsync(fileStream);
                }
                film.PosterPath = "images/" + uploaded.FileName;
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
