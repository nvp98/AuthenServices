using Microsoft.EntityFrameworkCore;
using AuthenServices.Models;

namespace AuthenServices.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Users> Users => Set<Users>();

    public DbSet<UserDomains> UserDomains => Set<UserDomains>();

    public DbSet<Domains> Domains => Set<Domains>();
}
