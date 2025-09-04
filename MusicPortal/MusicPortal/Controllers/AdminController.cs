using Microsoft.AspNetCore.Mvc;
using MusicPortal.BLL.Interfaces;
using MusicPortal.Filters;

namespace MusicPortal.Controllers
{
    [Culture]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        private bool IsAdmin()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";
            return isAdmin;
        }

        public async Task<IActionResult> Users()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            HttpContext.Session.SetString("path", Request.Path);
            var users = await _userService.GetAllUsers();
            return View(users);
        }

        public async Task<IActionResult> RegistrationRequests()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            HttpContext.Session.SetString("path", Request.Path);
            var requests = await _userService.GetInactiveUsers();
            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            var result = await _userService.DeleteUser(id);
            if (!result)
            {
                TempData["Error"] = "Failed to delete user.";
            }
            return RedirectToAction("Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateUser(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            var result = await _userService.ActivateUser(id);
            if (!result)
            {
                TempData["Error"] = "Failed to activate user.";
            }
            return RedirectToAction("Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAdmin(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            var result = await _userService.MakeAdmin(id);
            if (!result)
            {
                TempData["Error"] = "Failed to make user admin.";
            }
            return RedirectToAction("Users");
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