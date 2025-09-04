using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicPortal.BLL.DTO;
using MusicPortal.BLL.Interfaces;
using MusicPortal.Filters;

namespace MusicPortal.Controllers
{
    [Culture]
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

        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("path", Request.Path);
            var songs = await _songService.GetAllSongs();
            return View(songs);
        }

        public async Task<IActionResult> Create()
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Login", "Account");
            }
            HttpContext.Session.SetString("path", Request.Path);
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

            HttpContext.Session.SetString("path", Request.Path);
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
            HttpContext.Session.SetString("path", Request.Path);
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
            HttpContext.Session.SetString("path", Request.Path);
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
        public ActionResult ChangeCulture(string lang)
        {
            string? returnUrl = HttpContext.Session.GetString("path");

            List<string> cultures = ["en", "uk",];
            if (!cultures.Contains(lang))
            {
                lang = "en";
            }

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(10);
            Response.Cookies.Append("lang", lang, option);
            return Redirect(returnUrl);
        }
    }
}