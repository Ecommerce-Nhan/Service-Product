using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Categories;

namespace CategoryService.Infrastructure.Configurations.Categories;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(x => new { x.Id });
        builder.HasIndex(x => new { x.Id });
        builder.HasIndex(x => new { x.Code });
    }
}