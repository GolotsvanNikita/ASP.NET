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

        public void OnGet()
        {
            HttpContext.Session.SetString("path", Request.Path);
        }

        [BindProperty]
        public RegisterModelView User { get; set; } = default!;

        [ValidateAntiForgeryToken]
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                HttpContext.Session.SetString("path", Request.Path);
                return Page();
            }

            var user = new User
            {
                Name = User.Name,
                Password = _hasher.Hash(User.Password)
            };

            _repository.AddUser(user);
            _repository.SaveChanges();
            return RedirectToPage("/Login");
        }
    }
}