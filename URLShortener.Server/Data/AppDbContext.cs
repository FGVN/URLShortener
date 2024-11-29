using Microsoft.EntityFrameworkCore;
using URLShortener.Server.Models;
using URLShortener.Server.Services;

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
    public DbSet<URLData> URLs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .ToTable("Users")
            .HasDiscriminator<string>("UserType")
            .HasValue<Admin>("Admin")
            .HasValue<AuthorizedUser>("AuthorizedUser");

        modelBuilder.Entity<URLData>()
               .ToTable("URLs")
               .HasOne(u => u.Author) // Navigation property to User
               .WithMany() // Adjust this to the correct navigation property in the User entity if needed.
               .HasForeignKey(u => u.AuthorId)
               .OnDelete(DeleteBehavior.Cascade); // Adjust the delete behavior as required.

        // Configuring the owned UrlMetadata type within URLData.
        modelBuilder.Entity<URLData>()
            .OwnsOne(u => u.Metadata);

    }
}
