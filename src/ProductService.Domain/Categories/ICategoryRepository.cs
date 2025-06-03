using ProductService.Domain.Categories;
using SharedLibrary.Filters;
using SharedLibrary.Repositories.Abtractions;
using SharedLibrary.Wrappers;

namespace CategoryService.Domain.Categories;

public interface ICategoryRepository : IRepository<Category>;

public interface ICategoryReadOnlyRepository : IReadOnlyRepository<Category>
{
    Task<Category?> FindByCodeAsync(string code);
    Task<Category?> FindByNameAsync(string name);
    Task<PagedResponse<List<Category>>> GetPageAsync(PaginationFilter pageFilter);
}