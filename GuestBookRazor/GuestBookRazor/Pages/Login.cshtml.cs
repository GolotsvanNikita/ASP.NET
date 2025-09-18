using GuestBookRazor.Models;
using GuestBookRazor.Repositories;
using GuestBookRazor.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuestBookRazor.Pages
{
    public class LoginModel : PageModel
    {

        private IRepository _repository;
        private IPasswordHash _hasher;

        public LoginModel(IRepository repository, IPasswordHash hasher)
        {
            _repository = repository;
            _hasher = hasher;
        }

        public IActionResult Login()
        {
            HttpContext.Session.SetString("path", Request.Path);
            return Page();
        }

        [BindProperty]
        public LoginModelView User { get; set; }

        [ValidateAntiForgeryToken]
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var user = _repository.GetUserByName(User.Name);
                if (user == null || user.Password != _hasher.Hash(User.Password))
                {
                    ModelState.AddModelError("", Resources.Resource.WrongPassOrLog);
                    HttpContext.Session.SetString("path", Request.Path);
                }
                HttpContext.Session.SetString("Name", user.Name);
                return RedirectToAction("Index");
            }
            HttpContext.Session.SetString("path", Request.Path);
            return Page();
        }
    }
}
