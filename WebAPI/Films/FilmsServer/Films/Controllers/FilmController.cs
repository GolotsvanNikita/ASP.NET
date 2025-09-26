using Films.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Films.Controllers
{
    [ApiController]
    [Route("app/[controller]")]
    public class FilmController : Controller
    {
        ApplicationDbContext _context;
        IWebHostEnvironment _appEnvironment;

        public FilmController(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
        {
            return await _context.Films.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilm(int id)
        {
            var film = await _context.Films.FirstOrDefaultAsync(f => f.Id == id);
            if (film == null)
            {
                return NotFound();
            }
            return Ok(film);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Film film, IFormFile? uploaded)
        {
            if (id != film.Id) 
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Director,Year,Genre,Description")] Film film, IFormFile uploaded)
        {
            ModelState.Remove("uploaded");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = await _context.Films.AnyAsync(f =>
                f.Name != null && film.Name != null &&
                f.Director != null && film.Director != null &&
                string.Equals(f.Name, film.Name, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(f.Director, film.Director, StringComparison.OrdinalIgnoreCase) &&
                f.Year == film.Year);

            if (exists)
            {
                ModelState.AddModelError("", "This film already exists");
                return BadRequest(ModelState);
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
            else
            {
                film.PosterPath = "images/default.png";
            }

            _context.Films.Add(film);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFilm), new { id = film.Id }, film);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var film = await _context.Films.FirstOrDefaultAsync(f => f.Id == id);
            if (film == null)
            {
                return NotFound();
            }
            _context.Films.Remove(film);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
