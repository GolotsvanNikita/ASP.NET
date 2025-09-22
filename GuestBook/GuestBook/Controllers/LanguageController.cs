using Microsoft.AspNetCore.Mvc;

namespace GuestBook.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult Change(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                Response.Cookies.Append("lang", culture, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddYears(1),
                    HttpOnly = true
                });
            }

            string returnUrl = HttpContext.Session.GetString("path") ?? "/";

            if (Request.Headers.TryGetValue("X-Requested-With", out Microsoft.Extensions.Primitives.StringValues value) && value == "XMLHttpRequest")
            {
                return Json(new { success = true, redirectUrl = returnUrl });
            }

            return Redirect(returnUrl);
        }
    }
}