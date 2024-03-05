using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestServiceIf.Services;

namespace WestServiceIf.Controllers;

public class CompanyController(WSIDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Companies()
    {
        return View(await context.Companies.ToListAsync());
    }
}