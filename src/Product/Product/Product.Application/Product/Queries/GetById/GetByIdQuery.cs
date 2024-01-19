using ErrorOr;
using Microsoft.Extensions.Caching.Distributed;
using Product.Application.Product.Commands.Insert;
using Product.Application.Common.Abstractions;
using Product.Contracts.Product.GetById;
using Product.Core.Domain.Messaging;

namespace Product.Application.Product.Queries.GetById
{
    public record GetByIdQuery(Guid Id) : IQuery<ErrorOr<GetByIdResponse>>;


    public class GetByIdQueryCache : DistributedCache<GetByIdQuery, ErrorOr<GetByIdResponse>>
    {
        /*
        // expire the cache in five minutes from now regardless of how many times it's accessed in this time.
        protected override TimeSpan? AbsoluteExpirationRelativeToNow => new TimeSpan(0, 5, 0);
        */


        /// <summary>
        /// Each time the cached response is retrieved another 30 minutes will be added to the time before the cached
        /// response expires.
        /// </summary>
        protected override TimeSpan? SlidingExpiration => TimeSpan.FromSeconds(30);

        protected override Type CacheType => typeof(GetByIdResponse);

        public GetByIdQueryCache(IDistributedCache distributedCache) : base(distributedCache)
        {
        }

        protected override string GetCacheKeyIdentifier(GetByIdQuery request)
        {
            // cache every response where the Product id is different
            return request.Id.ToString();

            /*
            // in this case, return the whole list from cache. In production apps, the key could be a criteria object
            // including skip and take numbers to only cache a particular page etc.
            return string.Empty;
            */
        }

        protected override object GetCacheValue(ErrorOr<GetByIdResponse> request)
        {
            return request.Value;
        }

        protected override ErrorOr<GetByIdResponse> GetCacheReturnType(object value)
        {
            return (GetByIdResponse)value;
        }
    }



    /// <summary>
    /// Örnekleme için oluþturulmuþtur.
    /// </summary>
    public class InsertCommandGetByIdQueryCacheInvalidator : CacheInvalidator<InsertCommand, GetByIdQuery, ErrorOr<GetByIdResponse>>
    {
        public InsertCommandGetByIdQueryCacheInvalidator(ICache<GetByIdQuery, ErrorOr<GetByIdResponse>> cache) : base(cache)
        {
        }

        protected override string GetCacheKeyIdentifier(InsertCommand request)
        {
            return request.Name.ToString();
        }
    }
}