using SharedLibrary.Exceptions;
using System.Net;

namespace ProductService.Domain.Exceptions.Products;

public class ProductNotFoundException : BaseException
{
    public ProductNotFoundException(Guid id)
        : this(nameof(id), id.ToString())
    {
    }
    private ProductNotFoundException(string propertyName, string value)
       : base($"Product with {propertyName} '{value}' not found", HttpStatusCode.NotFound)
    {
    }
    public ProductNotFoundException()
        : base($"Product list not found", HttpStatusCode.NotFound)
    {
    }
}
