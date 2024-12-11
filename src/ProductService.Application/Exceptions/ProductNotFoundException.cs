using System.Net;

namespace ProductService.Application.Exceptions;

public class ProductNotFoundException : BaseException
{
    public ProductNotFoundException(Guid id)
        : base($"product with id {id} not found", HttpStatusCode.NotFound)
    {
    }
}
