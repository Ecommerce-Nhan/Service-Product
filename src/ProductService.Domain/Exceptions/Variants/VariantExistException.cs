using SharedLibrary.Exceptions;
using System.Net;

namespace ProductService.Domain.Exceptions.Variants;

public class VariantExistException : BaseException
{
    public VariantExistException(string code)
        : this(nameof(code), code)
    {
    }
    public VariantExistException(string name, bool isName = true)
         : this(nameof(name), name)
    {
    }
    private VariantExistException(string propertyName, string value)
       : base($"Variant with {propertyName} '{value}' already taken", HttpStatusCode.Conflict)
    {
    }
}

