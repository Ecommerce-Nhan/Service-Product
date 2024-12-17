using Serilog;
using System.Diagnostics;

namespace ProductService.Api.Middlewares;

public class RequestDurationMiddleware
{
    private readonly RequestDelegate _next;

    public RequestDurationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var watch = Stopwatch.StartNew();
        await _next.Invoke(context);
        watch.Stop();
        Log.Information("{duration}ms", watch.ElapsedMilliseconds);
    }
}
