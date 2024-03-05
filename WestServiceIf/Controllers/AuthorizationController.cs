using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WestServiceIf.Models;
using WestServiceIf.Models.DTOs;

namespace WestServiceIf.Controllers;

public class AuthorizationController(SignInManager<IdentityUser> signInManager) : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto login)
    {
        var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent: false, lockoutOnFailure: false);
        
        if (result.Succeeded)
        {
            return RedirectToAction(); // TODO: Redirect
        }

        return RedirectToAction("Error");
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete(".AspNetCore.Identity.Application");
        return RedirectToAction("Index", "Home");
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}