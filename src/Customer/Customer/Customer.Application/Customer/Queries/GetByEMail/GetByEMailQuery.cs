using Customer.Application.Customer.Commands.Insert;
using Customer.Application.Common.Abstractions;
using Customer.Contracts.Customer.GetByEMail;
using Customer.Core.Domain.Messaging;
using ErrorOr;
using Microsoft.Extensions.Caching.Distributed;

namespace Customer.Application.Customer.Queries.GetByEMail
{
    public record GetByEMailQuery(string Email) : IQuery<ErrorOr<GetByEMailResponse>>;


    public class GetByEMailQueryCache : DistributedCache<GetByEMailQuery, ErrorOr<GetByEMailResponse>>
    {
        protected override TimeSpan? SlidingExpiration => TimeSpan.FromSeconds(30);

        protected override Type CacheType => typeof(GetByEMailResponse);

        public GetByEMailQueryCache(IDistributedCache distributedCache) : base(distributedCache)
        {
        }

        protected override string GetCacheKeyIdentifier(GetByEMailQuery request)
        {
            return request.Email.ToString();
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


    public class InsertCommandGetByEMailQueryCacheInvalidator : CacheInvalidator<InsertCommand, GetByEMailQuery, ErrorOr<GetByEMailResponse>>
    {
        public InsertCommandGetByEMailQueryCacheInvalidator(ICache<GetByEMailQuery, ErrorOr<GetByEMailResponse>> cache) : base(cache)
        {
        }

        protected override string GetCacheKeyIdentifier(InsertCommand request)
        {
            return request.Email.ToString();
        }
    }

}