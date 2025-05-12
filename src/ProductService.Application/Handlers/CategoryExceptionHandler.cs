using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using CategoryService.Domain.Exceptions.Categories;
using SharedLibrary.Response.Identity;
using SharedLibrary.Wrappers;

namespace CategoryService.Application.Handlers;

public class CategoryExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
                                                Exception exception,
                                                CancellationToken cancellationToken)
    {
        if (exception is CategoryNotFoundException notFoundException)
            return await HandleExceptionAsync(httpContext, notFoundException, cancellationToken);
        if (exception is CategoryExistException existException)
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
