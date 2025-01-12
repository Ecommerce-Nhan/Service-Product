using SharedLibrary.Exceptions;
using System.Net;

namespace ProductService.Domain.Exceptions.Products;

public class ProductExistException : BaseException
{
    public ProductExistException(string code)
        : this(nameof(code), code)
    {
    }
    public ProductExistException(string name, bool isName = true)
         : this(nameof(name), name)
    {
    }
    private ProductExistException(string propertyName, string value)
       : base($"Product with {propertyName} '{value}' already taken", HttpStatusCode.Conflict)
    {
    }
}