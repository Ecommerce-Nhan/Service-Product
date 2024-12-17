using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using Serilog;

namespace ProductService.Common.CQRS.Behaviors;

public class CachingBehavior<TRequest, TResponse>(IDistributedCache cache) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheable
{
    public async Task<TResponse> Handle(TRequest request, 
                                        RequestHandlerDelegate<TResponse> next, 
                                        CancellationToken cancellationToken)
    {
        TResponse response;
        if (request.BypassCache) 
            return await next();

        async Task<TResponse> GetResponseAndAddToCache()
        {
            response = await next();
            if (response != null)
            {
                var slidingExpiration = request.SlidingExpirationInMinutes == 0 ? 30 : request.SlidingExpirationInMinutes;
                var absoluteExpiration = request.AbsoluteExpirationInMinutes == 0 ? 60 : request.AbsoluteExpirationInMinutes;

                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiration))
                                                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(absoluteExpiration));

                var serializedData = Encoding.Default.GetBytes(JsonSerializer.Serialize(response));
                await cache.SetAsync(request.CacheKey, serializedData, options, cancellationToken);
            }
            return response;
        }
        var cachedResponse = await cache.GetAsync(request.CacheKey, cancellationToken);
        if (cachedResponse != null)
        {
            response = JsonSerializer.Deserialize<TResponse>(Encoding.Default.GetString(cachedResponse))!;
            Log.Information("fetched from cache with key : {CacheKey}", request.CacheKey);
            cache.Refresh(request.CacheKey);
        }
        else
        {
            response = await GetResponseAndAddToCache();
            Log.Information("added to cache with key : {CacheKey}", request.CacheKey);
        }
        return response;
    }
}
