using TopFilms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace TopFilms.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public IndexModel(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IList<Film> Films { get; set; } = default!;

        [BindProperty]
        public Film NewFilm { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Films = await _context.Films.ToListAsync();
            NewFilm = new Film();
        }

        public async Task<IActionResult> OnPostCreateAsync(IFormFile uploaded)
        {
            ModelState.Remove("uploaded");

            if (!ModelState.IsValid)
            {
                Films = await _context.Films.ToListAsync();
                return Page();
            }

            bool exists = await _context.Films.AnyAsync(f =>
                (f.Name != null && NewFilm.Name != null && f.Name.ToLower() == NewFilm.Name.ToLower()) &&
                (f.Director != null && NewFilm.Director != null && f.Director.ToLower() == NewFilm.Director.ToLower()) &&
                f.Year == NewFilm.Year);

            if (exists)
            {
                ModelState.AddModelError("", "This film already exists");
                Films = await _context.Films.ToListAsync();
                return Page();
            }


            if (uploaded != null)
            {
                string path = "/images/" + uploaded.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploaded.CopyToAsync(fileStream);
                }
                NewFilm.PosterPath = "images/" + uploaded.FileName;
            }

            _context.Films.Add(NewFilm);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var film = await _context.Films.FindAsync(id);
            if (film != null)
            {
                _context.Films.Remove(film);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}