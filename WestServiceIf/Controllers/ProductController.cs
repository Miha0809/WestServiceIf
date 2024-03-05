using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestServiceIf.Models;
using WestServiceIf.Services;

namespace WestServiceIf.Controllers;

public class ProductController(WSIDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Products()
    {
        return View(await context.Products.OrderBy(p => p.Id).ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null && !IsExists(id))
        {
            return Error();
        }

        var product = await context.Products.FirstOrDefaultAsync(product => product.Id.Equals(id));

        return View(product);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add(Product product)
    {
        if (product is not null)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            return RedirectToAction("Products");
        }
        
        return Error();
    }

    private bool IsExists(int? id)
    {
        return context.Products.Find(id) != null;
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}