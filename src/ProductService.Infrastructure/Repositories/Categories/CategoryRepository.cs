using CategoryService.Domain.Categories;
using ProductService.Domain.Categories;
using ProductService.Infrastructure;
using ProductService.Infrastructure.Repositories;

namespace CategoryService.Infrastructure.Repositories.Categories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}