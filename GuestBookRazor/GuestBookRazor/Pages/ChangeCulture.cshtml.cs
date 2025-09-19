using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace GuestBookRazor.Pages
{
    public class ChangeCultureModel : PageModel
    {
        public IActionResult OnPost(string lang)
        {
            string? returnUrl = HttpContext.Session.GetString("path");

            List<string> cultures = new List<string>() { "en", "uk" };
            if (!cultures.Contains(lang))
            {
                lang = "en";
            }

            string cultureCookieValue = $"c={lang}|uic={lang}";

            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(10),
                Secure = Request.IsHttps,
                HttpOnly = true
            };
            Response.Cookies.Append("lang", cultureCookieValue, option);

            CultureInfo.CurrentCulture = new CultureInfo(lang);
            CultureInfo.CurrentUICulture = new CultureInfo(lang);

            return Redirect(returnUrl ?? "/Index");
        }
    }
}