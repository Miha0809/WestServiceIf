using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WestServiceIf.Services;

public class WSIDbContext(DbContextOptions<WSIDbContext> options) : IdentityDbContext(options)
{
    
}