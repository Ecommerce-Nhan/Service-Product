using ProductService.Common.Filters;
using ProductService.Common.Wrappers;

namespace ProductService.Domain.Abtractions;

public interface IReadOnlyRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<PagedResponse<List<T>>> GetPageAsync(PaginationFilter pageFilter);
}
