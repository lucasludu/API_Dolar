using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Behaviours
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICacheable
    {
        private readonly IMemoryCache _cache;

        public CachingBehavior(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(request.CacheKey, out TResponse response))
            {
                response = await next();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(request.Expiration ?? TimeSpan.FromMinutes(5));

                _cache.Set(request.CacheKey, response, cacheEntryOptions);
            }

            return response;
        }
    }
}
