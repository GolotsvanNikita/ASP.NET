using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicPortal.BLL.DTO;
using MusicPortal.BLL.Interfaces;
using MusicPortal.Filters;
using MusicPortal.Models;

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

        public async Task<IActionResult> Index(int page = 1, SortState sortOrder = SortState.NameAsc, string? searchString = null, int? genreId = null)
        {
            int pageSize = 6;

            var songs = await _songService.GetAllSongs();

            if (!string.IsNullOrEmpty(searchString))
            {
                songs = songs.Where(s => s.Name!.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            if (genreId.HasValue)
            {
                songs = songs.Where(s => s.GenreId == genreId.Value);
            }

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

            var sortedItems = sortOrder switch
            {
                SortState.NameDesc => items.OrderByDescending(s => s.Name),
                SortState.DurationAsc => items.OrderBy(s => s.Duration ?? 0),
                SortState.DurationDesc => items.OrderByDescending(s => s.Duration ?? 0),
                _ => items.OrderBy(s => s.Name),
            };

            var itemList = sortedItems.ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            SortViewModel sortViewModel = new SortViewModel(sortOrder);
            IndexViewModel viewModel = new IndexViewModel(itemList, pageViewModel, sortViewModel, searchString ?? "", genreId);

            ViewData["Genres"] = new SelectList(await _genreService.GetAllGenres(), "GenreId", "Name", genreId);

            HttpContext.Session.SetString("path", Request.Path);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDuration([FromBody] SongDurationUpdateModel model)
        {
            if (model == null || model.Id <= 0 || model.Duration <= 0 || double.IsNaN(model.Duration))
            {
                return BadRequest("Invalid data");
            }

            var song = await _songService.GetSongById(model.Id);
            if (song == null)
            {
                return NotFound();
            }

            song.Duration = model.Duration;

            var updated = await _songService.UpdateSong(song, null);
            if (!updated)
            {
                return StatusCode(500, new { message = "Error saving duration" });
            }

            return Ok(new { message = "Duration saved", duration = model.Duration });
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
        public async Task<IActionResult> Create(SongDTO songDto, IFormFile upload)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Login", "Account");
            }

            songDto.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (ModelState.IsValid)
            {
                try
                {
                    var added = await _songService.AddSong(songDto, upload);
                    if (added)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unexpected error while saving the song.");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unexpected error while saving the song.");
                }
            }

            HttpContext.Session.SetString("path", Request.Path);
            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenres(), "GenreId", "Name", songDto.GenreId);
            return View(songDto);
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
        [HttpPost]
        public async Task<IActionResult> Edit(SongDTO songDto, IFormFile? upload)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                HttpContext.Session.SetString("path", Request.Path);
                ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenres(), "GenreId", "Name", songDto.GenreId);
                return View(songDto);
            }
            var updated = await _songService.UpdateSong(songDto, upload);
            if (updated)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error updating song. Please try again.");
            HttpContext.Session.SetString("path", Request.Path);
            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenres(), "GenreId", "Name", songDto.GenreId);
            return View(songDto);
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