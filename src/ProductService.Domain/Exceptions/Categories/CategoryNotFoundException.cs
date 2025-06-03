using SharedLibrary.Exceptions;
using System.Net;

namespace CategoryService.Domain.Exceptions.Categories;

public class CategoryNotFoundException : BaseException
{
    public CategoryNotFoundException(Guid id)
        : this(nameof(id), id.ToString())
    {
    }
    private CategoryNotFoundException(string propertyName, string value)
       : base($"Category with {propertyName} '{value}' not found", HttpStatusCode.NotFound)
    {
    }
    public CategoryNotFoundException()
        : base($"Category list not found", HttpStatusCode.NotFound)
    {
    }
}
