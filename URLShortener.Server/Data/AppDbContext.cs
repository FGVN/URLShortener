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
    public DbSet<AboutUs> AboutUs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AboutUs>()
            .ToTable("AboutUs")
            .HasOne(a => a.Author)
            .WithMany();


        modelBuilder.Entity<User>()
            .ToTable("Users")
            .HasDiscriminator<string>("UserType")
            .HasValue<Admin>("Admin")
            .HasValue<AuthorizedUser>("AuthorizedUser");

        modelBuilder.Entity<URLData>()
               .ToTable("URLs")
               .HasOne(u => u.Author) 
               .WithMany() 
               .HasForeignKey(u => u.AuthorId)
               .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<URLData>()
            .OwnsOne(u => u.Metadata);

    }
}
