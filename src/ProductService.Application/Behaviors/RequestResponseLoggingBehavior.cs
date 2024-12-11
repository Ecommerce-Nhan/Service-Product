using MediatR;
using Serilog;
using System.Text.Json;

namespace ProductService.Application.Behaviors;

public class RequestResponseLoggingBehavior<TRequest, TResponse>() 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : class
{
    public async Task<TResponse> Handle(TRequest request, 
                                        RequestHandlerDelegate<TResponse> next, 
                                        CancellationToken cancellationToken)
    {
        var correlationId = Guid.NewGuid();
        var requestJson = JsonSerializer.Serialize(request);
        Log.Information("Handling request {CorrelationID}: {Request}", correlationId, requestJson);

        var response = await next();
        var responseJson = JsonSerializer.Serialize(response);
        Log.Information("Response for {Correlation}: {Response}", correlationId, responseJson);

        return response;
    }
}
