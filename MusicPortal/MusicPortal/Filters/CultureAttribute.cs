using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace MusicPortal.Filters
{
    public class CultureAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string? cultureName = null;

            var cultureCookie = context.HttpContext.Request.Cookies["lang"];
            if (cultureCookie != null)
            {
                cultureName = cultureCookie;
            }
            else 
            {
                cultureName = "en";
            }

            List<string> cultures = ["en", "uk"];
            if (!cultures.Contains(cultureName))
            {
                cultureName = "en";
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
        }
    }
}
