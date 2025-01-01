using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Products;
using ProductService.Domain.Variants;

namespace ProductService.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Variant> Variants { get; set; }
}
