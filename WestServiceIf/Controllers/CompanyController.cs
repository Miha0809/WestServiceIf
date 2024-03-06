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
        var companies = await context.Companies.ToListAsync();
        return View(companies);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Company company)
    {
        if (ValidateCompanyOnError(company))
        {
            return NotFound("Назва або опис компанії пустий");
        }

        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();

        return RedirectToAction("Companies");
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var company = await context.Companies.FindAsync(id);
        
        if (ValidateCompanyOnError(company))
        {
            return NotFound("Компанія не знайдена");
        }
                
        context.Companies.Remove(company);
        await context.SaveChangesAsync();

        return RedirectToAction("Companies");
    }
    
    private bool ValidateCompanyOnError(Company? company)
    {
        return company is null || string.IsNullOrWhiteSpace(company.Description) ||
               string.IsNullOrWhiteSpace(company.Name);
    }
}