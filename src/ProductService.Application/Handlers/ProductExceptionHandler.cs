using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using ProductService.Domain.Exceptions.Products;
using SharedLibrary.Wrappers;

namespace ProductService.Application.Handlers;

public class ProductExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
                                                Exception exception,
                                                CancellationToken cancellationToken)
    {
        if (exception is ProductNotFoundException notFoundException)
            return await HandleExceptionAsync(httpContext, notFoundException, cancellationToken);
        if (exception is ProductExistException existException)
            return await HandleExceptionAsync(httpContext, existException, cancellationToken);

        return false;
    }
    private async Task<bool> HandleExceptionAsync(HttpContext httpContext,
                                                  Exception exception,
                                                  CancellationToken cancellationToken)
    {
        var response = new Response<object>
        {
            Succeeded = false,
            Data = null,
            Errors = new[] { exception.Message },
            Message = "Invalid request",
        };
        httpContext.Response.StatusCode = (int)((dynamic)exception).StatusCode;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken)
                                  .ConfigureAwait(false);

        return true;
    }
}
