using TopFilms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TopFilms.Pages
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public EditModel(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        [BindProperty]
        public Film Film { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || (Film = await _context.Films.FindAsync(id)) == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile uploaded, int? id)
        {
            if (id == null || (Film = await _context.Films.FindAsync(id)) == null)
            {
                return NotFound();
            }

            ModelState.Remove("uploaded");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await TryUpdateModelAsync(Film, "Film", 
                f => f.Name, f => f.Director, f => f.Genre, f => f.Year, f => f.Description);

            if (uploaded != null)
            {
                string imagesPath = Path.Combine(_appEnvironment.WebRootPath, "images");

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploaded.FileName).ToLower();
                string path = Path.Combine(imagesPath, fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploaded.CopyToAsync(fileStream);
                }
                Film.PosterPath = "images/" + fileName;
            }

            _context.Update(Film);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}