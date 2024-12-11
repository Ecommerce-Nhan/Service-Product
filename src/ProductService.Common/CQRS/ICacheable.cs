namespace ProductService.Common.CQRS;

public interface ICacheable
{
    bool BypassCache { get; }
    string CacheKey { get; }
    int SlidingExpirationInMinutes { get; }
    int AbsoluteExpirationInMinutes { get; }
}
