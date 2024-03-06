using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        if (login.Email is null || login.Password is null)
        {
            return NotFound("Емейл або пароль не введені");
        }
        
        var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent: false, lockoutOnFailure: false);
        
        if (result.Succeeded)
        {
            return RedirectToAction("Products", "Product");
        }

        return NotFound("Емейл або пароль не коректний");
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete(".AspNetCore.Identity.Application");
        return RedirectToAction("Products", "Product");
    }
}