using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicPortal.BLL.DTO;
using MusicPortal.BLL.Interfaces;
using MusicPortal.BLL.Services;

namespace MusicPortal.Controllers
{
    public class SongsController : Controller
    {
        private readonly ISongService _songService;
        private readonly IGenreService _genreService;

        public SongsController(ISongService songService, IGenreService genreService)
        {
            _songService = songService;
            _genreService = genreService;
        }

        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "True";
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 6;

            var songs = await _songService.GetAllSongs();
            var count = songs.Count();

            if (page < 1)
            {
                page = 1;
            }

            int totalPages = (int)Math.Ceiling((double)count / pageSize);

            if (page > totalPages)
            {
                page = totalPages;
            }

            var items = songs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel(items, pageViewModel);
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenres(), "GenreId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SongDTO song, IFormFile upload)
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

                    await _songService.AddSong(song, upload);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unexpected error while saving the song.");
                }
            }

            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenres(), "GenreId", "Name", song.GenreId);
            return View(song);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            var song = await _songService.GetSongById(id);
            if (song == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenres(), "GenreId", "Name", song.GenreId);
            return View(song);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SongDTO song, IFormFile? upload)
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

                await _songService.UpdateSong(song, upload);
                return RedirectToAction("Index");
            }
            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenres(), "GenreId", "Name", song.GenreId);
            return View(song);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            var song = await _songService.GetSongById(id);
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
                await _songService.DeleteSong(id);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Download(int id)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Login", "Account");
            }
            var song = await _songService.GetSongById(id);
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