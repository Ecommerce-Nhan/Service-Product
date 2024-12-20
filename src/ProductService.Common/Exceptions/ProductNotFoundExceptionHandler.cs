using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace ProductService.Common.Exceptions;

public class ProductNotFoundExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ProductNotFoundException e)
        {
            return false;
        }
        // Implement logic
        await Task.CompletedTask;

        return true;
    }
}
