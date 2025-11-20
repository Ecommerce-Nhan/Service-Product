using SharedLibrary.Exceptions;
using System.Net;

namespace ProductService.Domain.Exceptions.Variants;

public class VariantNotFoundException : BaseException
{
    public VariantNotFoundException(Guid id)
        : this(nameof(id), id.ToString())
    {
    }
    private VariantNotFoundException(string propertyName, string value)
       : base($"Variant with {propertyName} '{value}' not found", HttpStatusCode.NotFound)
    {
    }
    public VariantNotFoundException()
        : base($"Variant list not found", HttpStatusCode.NotFound)
    {
    }
}

