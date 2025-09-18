using GuestBookRazor.Models;
using GuestBookRazor.Repositories;
using GuestBookRazor.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuestBookRazor.Pages
{
    public class RegisterModel : PageModel
    {
        private IRepository _repository;
        private IPasswordHash _hasher;

        public RegisterModel(IRepository repository, IPasswordHash hasher)
        {
            _repository = repository;
            _hasher = hasher;
        }
        public IActionResult Register()
        {
            HttpContext.Session.SetString("path", Request.Path);
            return Page();
        }

        [BindProperty]
        public RegisterModelView User { get; set; } = default!;

        [ValidateAntiForgeryToken]
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Name = User.Name,
                    Password = _hasher.Hash(User.Password)
                };

                _repository.SaveChanges();
                _repository.AddUser(user);
                RedirectToAction("Login");
            }
            HttpContext.Session.SetString("path", Request.Path);
            return Page();
        }
    }
}
