using Microsoft.EntityFrameworkCore;
using URLShortener.Server.Models;

namespace URLShortener.Server.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<AuthorizedUser> AuthorizedUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .ToTable("Users")
            .HasDiscriminator<string>("UserType")
            .HasValue<Admin>("Admin")
            .HasValue<AuthorizedUser>("AuthorizedUser");
    }

}
