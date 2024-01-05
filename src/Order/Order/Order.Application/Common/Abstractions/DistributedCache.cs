using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Order.Core.Domain.Messaging;
using System.Text.Json;

namespace Order.Application.Common.Abstractions
{
    /// <summary>
    /// Implement this class when you want your cached request response to be stored in a distributed cached
    /// </summary>
    /// <typeparam name="TRequest">The type of the request who's response will be cached.</typeparam>
    /// <typeparam name="TResponse">The type of the response of the request that will be cached.</typeparam>
    public abstract class DistributedCache<TRequest, TResponse> : ICache<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IDistributedCache _distributedCache;

        protected virtual DateTime? AbsoluteExpiration { get; }
        protected virtual TimeSpan? AbsoluteExpirationRelativeToNow { get; }
        protected virtual TimeSpan? SlidingExpiration { get; }
        protected virtual Type CacheType { get; }

        protected DistributedCache(
            IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// Override and return a string key to uniquely identify the cached response
        /// </summary>
        /// <param name="request">The type of the request who's response will be cached.</param>
        /// <returns></returns>
        protected abstract string GetCacheKeyIdentifier(TRequest request);

        protected abstract object GetCacheValue(TResponse request);

        protected abstract TResponse GetCacheReturnType(object value);

        private JsonSerializerOptions JsonOptions
        {
            get
            {
                return new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
            }
        }

        private static string GetCacheKey(string id)
        {
            return $"{typeof(TRequest).FullName}:{id}";
        }

        private string GetCacheKey(TRequest request)
        {
            return GetCacheKey(GetCacheKeyIdentifier(request));
        }

        public virtual async Task<TResponse> Get(TRequest request)
        {
            var cacheResponse = await _distributedCache.GetAsync(GetCacheKey(request));
            if (cacheResponse == null)
            {
                return default(TResponse);
            }

            var responseJson = JsonSerializer.Deserialize(cacheResponse, CacheType, JsonOptions);
            if (responseJson == null)
            {
                return default(TResponse);
            }

            return GetCacheReturnType(responseJson);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual async Task Set(TRequest request, TResponse value)
        {
            await _distributedCache.SetAsync(
                GetCacheKey(request),
                JsonSerializer.SerializeToUtf8Bytes(GetCacheValue(value), JsonOptions),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = AbsoluteExpiration,
                    AbsoluteExpirationRelativeToNow = AbsoluteExpirationRelativeToNow,
                    SlidingExpiration = SlidingExpiration
                });
        }

        /// <summary>
        /// Removes the response from cache using the CacheKeyIdentifier from the Request
        /// </summary>
        /// <param name="cacheKeyIdentifier">A string identifier that uniquely identifies the response to be removed</param>
        /// <returns></returns>
        public async Task Remove(string cacheKeyIdentifier)
        {
            await _distributedCache.RemoveAsync(GetCacheKey(cacheKeyIdentifier));
        }
    }
}
