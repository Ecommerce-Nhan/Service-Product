using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using ProductService.Domain.Exceptions.Products;
using SharedLibrary.Response.Identity;
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
        var response = await Response<PermissionResponse>.FailAsync(new List<string> { exception.Message });
        httpContext.Response.StatusCode = (int)((dynamic)exception).StatusCode;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken)
                                  .ConfigureAwait(false);

        return true;
    }
}
