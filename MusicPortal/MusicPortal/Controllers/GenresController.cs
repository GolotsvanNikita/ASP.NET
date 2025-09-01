using Microsoft.AspNetCore.Mvc;
using MusicPortal.Models;
using MusicPortal.Repositories;
using System.Threading.Tasks;

namespace MusicPortal.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreRepository _genreRepository;

        public GenresController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
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
            var genres = await _genreRepository.GetAllGenresAsync();
            return View(genres);
        }

        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Genre genre)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                await _genreRepository.AddGenreAsync(genre);
                return RedirectToAction("Index");
            }
            return View(genre);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            var genre = await _genreRepository.GetGenreByIdAsync(id);
            if (genre == null) return NotFound();
            return View(genre);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Genre genre)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                await _genreRepository.UpdateGenreAsync(genre);
                return RedirectToAction("Index");
            }
            return View(genre);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            await _genreRepository.DeleteGenreAsync(id);
            return RedirectToAction("Index");
        }
    }
}