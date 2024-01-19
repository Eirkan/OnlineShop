using Customer.Application.Customer.Commands.Insert;
using Customer.Application.Common.Abstractions;
using Customer.Contracts.Customer.GetByEMail;
using Customer.Core.Domain.Messaging;
using ErrorOr;
using Microsoft.Extensions.Caching.Distributed;

namespace Customer.Application.Customer.Queries.GetByEMail
{
    public record GetByEMailQuery(string Email) : IQuery<ErrorOr<GetByEMailResponse>>;


    public class LoginQueryCache : DistributedCache<GetByEMailQuery, ErrorOr<GetByEMailResponse>>
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

        protected override Type CacheType => typeof(GetByEMailResponse);

        public LoginQueryCache(IDistributedCache distributedCache) : base(distributedCache)
        {
        }

        protected override string GetCacheKeyIdentifier(GetByEMailQuery request)
        {
            return request.Email.ToString();

            /*
            // in this case, return the whole list from cache. In production apps, the key could be a criteria object
            // including skip and take numbers to only cache a particular page etc.
            return string.Empty;
            */
        }

        protected override object GetCacheValue(ErrorOr<GetByEMailResponse> request)
        {
            return request.Value;
        }

        protected override ErrorOr<GetByEMailResponse> GetCacheReturnType(object value)
        {
            return (GetByEMailResponse)value;
        }
    }


    /// <summary>
    /// Örnekleme için oluþturulmuþtur.
    /// </summary>
    public class RegisterCommandLoginQueryCacheInvalidator : CacheInvalidator<InsertCommand, GetByEMailQuery, ErrorOr<GetByEMailResponse>>
    {
        public RegisterCommandLoginQueryCacheInvalidator(ICache<GetByEMailQuery, ErrorOr<GetByEMailResponse>> cache) : base(cache)
        {
        }

        protected override string GetCacheKeyIdentifier(InsertCommand request)
        {
            return request.Email.ToString();
        }
    }

}