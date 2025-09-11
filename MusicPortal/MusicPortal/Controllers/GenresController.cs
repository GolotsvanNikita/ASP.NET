using Microsoft.AspNetCore.Mvc;
using MusicPortal.BLL.DTO;
using MusicPortal.BLL.Interfaces;
using MusicPortal.Filters;

namespace MusicPortal.Controllers
{
    [Culture]
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "True";
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            HttpContext.Session.SetString("path", Request.Path);
            var genres = await _genreService.GetAllGenres();
            return View(genres);
        }

        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            HttpContext.Session.SetString("path", Request.Path);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GenreDTO genre)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                await _genreService.AddGenre(genre);
                return RedirectToAction("Index");
            }
            HttpContext.Session.SetString("path", Request.Path);
            return View(genre);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            var genre = await _genreService.GetGenreById(id);
            if (genre == null) 
            {
                return NotFound();
            }
            HttpContext.Session.SetString("path", Request.Path);
            return View(genre);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GenreDTO genre)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                await _genreService.UpdateGenre(genre);
                return RedirectToAction("Index");
            }
            HttpContext.Session.SetString("path", Request.Path);
            return View(genre);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            await _genreService.DeleteGenre(id);
            return RedirectToAction("Index");
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