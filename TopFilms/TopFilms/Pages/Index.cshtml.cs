using Films.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopFilms.Models;

namespace TopFilms.Pages
{
    public class IndexModel : PageModel
    {

        ApplicationDbContext _context;
        IWebHostEnvironment _appEnvironment;

        public IndexModel(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IList<Film> Film { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            var film = await _context.Films.FirstOrDefault(film => film.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            Film = film;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var film = _context.Films.FirstOrDefault(film => film.Id == id);
            if (film != null)
            {
                _context.Films.Remove(film);
                await _context.SaveChangesAsync();
            }
            return Page();
        }
    }
}
