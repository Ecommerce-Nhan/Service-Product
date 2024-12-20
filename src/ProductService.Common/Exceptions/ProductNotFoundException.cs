using System.Net;

namespace ProductService.Common.Exceptions;

public class ProductNotFoundException : BaseException
{
    public ProductNotFoundException(Guid id)
        : this(nameof(id), id.ToString())
    {
    }
    public ProductNotFoundException(string code)
        : this(nameof(code), code)
    {
    }
    public ProductNotFoundException(string name, bool isName = true)
         : this(nameof(name), name)
    {
    }
    private ProductNotFoundException(string propertyName, string value)
       : base($"Product with {propertyName} '{value}' not found", HttpStatusCode.NotFound)
    {
    }
    public ProductNotFoundException()
        : base($"product list not found", HttpStatusCode.NotFound)
    {
    }
}
