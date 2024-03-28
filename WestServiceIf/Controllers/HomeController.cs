using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace WestServiceIf.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return RedirectToAction("Products", "Product");
    }

    [HttpGet]
    public async Task<IActionResult> AboutUs()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AboutProduction()
    {
        return View();
    }

    public IActionResult SetCulture(string culture, string returnUrl)
    {
        if (string.IsNullOrEmpty(culture))
        {
            culture = "uk";
        }

        HttpContext.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        if (Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }
}