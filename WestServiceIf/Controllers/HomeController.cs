using Microsoft.AspNetCore.Mvc;

namespace WestServiceIf.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("Products", "Product");
    }

    [HttpGet]
    public IActionResult AboutUs()
    {
        return View();
    }
}