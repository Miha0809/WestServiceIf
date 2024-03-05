using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestServiceIf.Models;
using WestServiceIf.Services;

namespace WestServiceIf.Controllers;

[Authorize]
public class CompanyController(WSIDbContext context) : Controller
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Companies()
    {
        return View(await context.Companies.ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Company company)
    {
        if (company is null)
        {
            return View(company);
        }

        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();

        return RedirectToAction("Companies");
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var company = await context.Companies.FindAsync(id);
        
        if (company is null)
        {
            return Error();
        }
                
        context.Companies.Remove(company);
        await context.SaveChangesAsync();

        return RedirectToAction("Companies");
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}