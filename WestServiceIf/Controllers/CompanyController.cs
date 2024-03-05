using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestServiceIf.Models;
using WestServiceIf.Services;

namespace WestServiceIf.Controllers;

public class CompanyController(WSIDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Companies()
    {
        return View(await context.Companies.ToListAsync());
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete(int? id)
    {
        var company = await context.Companies.FindAsync(id);
        
        if (id is null || company is null)
        {
            return Error();
        }
                
        context.Companies.Remove(company);
        context.SaveChanges();

        return RedirectToAction("Companies");
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}