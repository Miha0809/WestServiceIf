using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestServiceIf.Models;
using WestServiceIf.Services;

namespace WestServiceIf.Controllers;

public class ProductController(WSIDbContext context, IWebHostEnvironment appEnvironment) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Products()
    {
        var producs = await context.Products.OrderBy(p => p.Id).ToListAsync();
        return View(producs);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (!ProductExists(id))
        {
            return View("Products");
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
    public async Task<IActionResult> Add(Product product, List<IFormFile> images)
    {
        if (ModelState.IsValid && product is not null)
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

        return Error();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!ProductExists(id))
        {
            return Error();
        }
        
        return View(await context.Products.FindAsync(id));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(Product model,  List<IFormFile> newImages, List<int> deleteImages)
    {
        if (model is null)
        {
            return Error();
        }
        
        if (ModelState.IsValid)
        {
            try
            {
                var product = await context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id.Equals(model.Id));
                if (product == null)
                {
                    return NotFound();
                }

                product.Title = model.Title;

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
            return RedirectToAction(nameof(Products));
        }
        return View(model);
    }

    private bool ProductExists(int id)
    {
        return context.Products.Any(e => e.Id == id);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}