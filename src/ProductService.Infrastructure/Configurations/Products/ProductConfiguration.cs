using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Products;

namespace ProductService.Infrastructure.Configurations.Products;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(x => new { x.Id });
        builder.HasIndex(x => new { x.Id });
        builder.HasIndex(x => new { x.Code });
    }
}
