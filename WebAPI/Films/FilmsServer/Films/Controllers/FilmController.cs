using Films.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Films.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] Film film, [FromForm] IFormFile? uploaded)
        {
            ModelState.Remove("uploaded");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = await _context.Films.AnyAsync(f =>
                f.Name != null &&
                f.Director != null &&
                f.Name.ToLower() == film.Name!.ToLower() &&
                f.Director.ToLower() == film.Director!.ToLower() &&
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
