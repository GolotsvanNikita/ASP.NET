using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicPortal.Models;
using System.Threading.Tasks;

namespace MusicPortal.Controllers
{
    public class SongsController : Controller
    {
        private MusicPortalContext _context;

        public SongsController(MusicPortalContext context) 
        {
            _context = context;
        }

        public async Task<IActionResult> Index() 
        {
            var songs = await _context.Songs.Include(s => s.Genre).Include(s => s.User).ToListAsync();
            return View(songs);
        }

        public IActionResult Create() 
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Song song, IFormFile upload) 
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.Length > 0)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "~/Uploads", upload.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await upload.CopyToAsync(stream);
                    }

                    song.FilePath = "/Uploads/" + upload.FileName;
                }

                _context.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "Name", song.GenreId);
            return View(song);
        }
    }
}
