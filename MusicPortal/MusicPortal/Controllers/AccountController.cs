using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPortal.Models;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MusicPortal.Controllers
{
    public class AccountController : Controller
    {
        private MusicPortalContext _context;

        public AccountController(MusicPortalContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user, string password)
        {
            if (ModelState.IsValid) 
            {
                user.PasswordHash = GetHash(password);
                user.IsActive = false;
                _context.Users.Add(user);
                _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }

            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string login, string password) 
        {
            string hash = GetHash(password);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.PasswordHash == hash);

            if (user != null && user.IsActive)
            {
                HttpContext.Session.SetString("Id", user.Id.ToString());
                HttpContext.Session.SetString("Login", user.Login);
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

                return RedirectToAction("Index", "Songs");
            }

            ModelState.AddModelError("", "Incorrect Login or Password or your account is no activated");
            return View();
        }

        public IActionResult Logout() 
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private string GetHash(string password) 
        {
            using (var sha = SHA256.Create())
            {
                byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(data);
            }
        }
    }
}
