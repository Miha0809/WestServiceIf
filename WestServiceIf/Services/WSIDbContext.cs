using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WestServiceIf.Models;

namespace WestServiceIf.Services;

public class WSIDbContext(DbContextOptions<WSIDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Product> Products { get; set; }
}