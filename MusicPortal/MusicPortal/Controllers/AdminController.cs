using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using MusicPortal.Models;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace MusicPortal.Controllers
{
    public class AdminController : Controller
    {
        private MusicPortalContext _context;

        public AdminController(MusicPortalContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Users() 
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id) 
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null) 
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Users");
        }
    }
}
