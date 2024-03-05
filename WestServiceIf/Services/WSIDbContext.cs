using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WestServiceIf.Models;

namespace WestServiceIf.Services;

public class WSIDbContext(DbContextOptions<WSIDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Company> Companies { get; set; }
}