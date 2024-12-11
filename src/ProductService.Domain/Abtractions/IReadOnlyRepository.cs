using System.Linq.Expressions;

namespace ProductService.Domain.Abtractions;

public interface IReadOnlyRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    IEnumerable<T> GetAllAsync();
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);
}
