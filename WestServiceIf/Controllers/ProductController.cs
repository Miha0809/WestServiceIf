using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestServiceIf.Models;
using WestServiceIf.Services;

namespace WestServiceIf.Controllers;

[Authorize]
public class ProductController(WSIDbContext context) : Controller
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Products()
    {
        var products = await context.Products.OrderBy(p => p.Id).ToListAsync();
        return View(products);
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var product = await context.Products.FirstOrDefaultAsync(product => product.Id.Equals(id));

        if (ValidateCompanyOnError(product))
        {
            return NotFound($"Продукта з id {id} не існує");
        }

        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(Product product, List<IFormFile> images)
    {
        if (ModelState.IsValid && ValidateCompanyOnError(product))
        {
            foreach (var imageFile in images)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    var image = new Image
                    {
                        FileName = imageFile.FileName,
                        ImageData = memoryStream.ToArray()
                    };
                    product.Images.Add(image);
                }
            }

            context.Products.Add(product);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Products));
        }

        return NotFound("Назва продукта або опис пусті");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await context.Products.FindAsync(id);
        
        if (ValidateCompanyOnError(product))
        {
            return NotFound($"Продукта з id {id} не існує");
        }
        
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Product model,  List<IFormFile> newImages, List<int> deleteImages)
    {
        if (ValidateCompanyOnError(model))
        {
            return NotFound("Назва продукта або опис пусті");
        }
        
        if (ModelState.IsValid)
        {
            try
            {
                var product = await context.Products.FindAsync(model.Id);
                
                if (product == null)
                {
                    return NotFound();
                }

                product.Title = model.Title;
                product.Description = model.Description;

                if (newImages != null && newImages.Any())
                {
                    foreach (var file in newImages)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            var image = new Image()
                            {
                                FileName = file.FileName,
                                ImageData = memoryStream.ToArray()
                            };
                            product.Images.Add(image);
                        }
                    }
                }

                if (deleteImages != null && deleteImages.Any())
                {
                    foreach (var imageId in deleteImages)
                    {
                        var imageToRemove = product.Images.FirstOrDefault(img => img.Id == imageId);
                        if (imageToRemove != null)
                        {
                            product.Images.Remove(imageToRemove);
                        }
                    }
                }

                context.Update(product);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return RedirectToAction("Products");
        }
        
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await context.Products.FindAsync(id);
        
        if (ValidateCompanyOnError(product))
        {
            return NotFound("Назва продукта або опис пусті");
        }
                
        context.Products.Remove(product!);
        await context.SaveChangesAsync();

        return RedirectToAction("Products");
    }

    private bool ValidateCompanyOnError(Product? product)
    {
        return product is null || string.IsNullOrWhiteSpace(product.Title) ||
               string.IsNullOrWhiteSpace(product.Description);
    }
    
    private bool ProductExists(int id)
    {
        return context.Products.Any(p => p.Id.Equals(id));
    }
}