using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicPortal.Models;
using MusicPortal.Repositories;
using System.IO;
using System.Threading.Tasks;

namespace MusicPortal.Controllers
{
    public class SongsController : Controller
    {
        private readonly ISongRepository _songRepository;
        private readonly IGenreRepository _genreRepository;

        public SongsController(ISongRepository songRepository, IGenreRepository genreRepository)
        {
            _songRepository = songRepository;
            _genreRepository = genreRepository;
        }

        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "True";
        }

        public async Task<IActionResult> Index()
        {
            var songs = await _songRepository.GetAllSongsAsync();
            return View(songs);
        }

        public async Task<IActionResult> Create()
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["GenreId"] = new SelectList(await _genreRepository.GetAllGenresAsync(), "GenreId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Song song, IFormFile upload)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Login", "Account");
            }

            song.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (ModelState.IsValid)
            {
                try
                {
                    if (upload != null && upload.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var filePath = Path.Combine(uploadsFolder, upload.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await upload.CopyToAsync(stream);
                        }

                        song.FilePath = "/Uploads/" + upload.FileName;
                    }

                    await _songRepository.AddSongAsync(song);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unexpected error while saving the song.");
                }
            }

            ViewData["GenreId"] = new SelectList(await _genreRepository.GetAllGenresAsync(), "GenreId", "Name", song.GenreId);
            return View(song);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            var song = await _songRepository.GetSongByIdAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(await _genreRepository.GetAllGenresAsync(), "GenreId", "Name", song.GenreId);
            return View(song);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Song song, IFormFile? upload)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                if (upload != null && upload.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads");
                    var filePath = Path.Combine(uploadsFolder, upload.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await upload.CopyToAsync(stream);
                    }
                    song.FilePath = "/Uploads/" + upload.FileName;
                }

                await _songRepository.UpdateSongAsync(song);
                return RedirectToAction("Index");
            }
            ViewData["GenreId"] = new SelectList(await _genreRepository.GetAllGenresAsync(), "GenreId", "Name", song.GenreId);
            return View(song);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            var song = await _songRepository.GetSongByIdAsync(id);
            if (song != null)
            {
                if (!string.IsNullOrEmpty(song.FilePath))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + song.FilePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                await _songRepository.DeleteSongAsync(id);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Download(int id)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Login", "Account");
            }
            var song = await _songRepository.GetSongByIdAsync(id);
            if (song == null || string.IsNullOrEmpty(song.FilePath))
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + song.FilePath);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "audio/mpeg", Path.GetFileName(filePath));
        }
    }
}