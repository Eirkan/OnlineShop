using ErrorOr;
using Microsoft.Extensions.Caching.Distributed;
using Product.Application.Authentication.Commands.Register;
using Product.Application.Common.Abstractions;
using Product.Contracts.Authentication.Login;
using Product.Core.Domain.Messaging;

namespace Product.Application.Authentication.Queries.Login
{
    public record LoginQuery(
        string Email,
        string Password) : IQuery<ErrorOr<LoginResponse>>;


    public class LoginQueryCache : DistributedCache<LoginQuery, ErrorOr<LoginResponse>>
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

        protected override Type CacheType => typeof(LoginResponse);

        public LoginQueryCache(IDistributedCache distributedCache) : base(distributedCache)
        {
        }

        protected override string GetCacheKeyIdentifier(LoginQuery request)
        {
            // cache every response where the user id is different
            return request.Email.ToString();

            /*
            // in this case, return the whole list from cache. In production apps, the key could be a criteria object
            // including skip and take numbers to only cache a particular page etc.
            return string.Empty;
            */
        }

        protected override object GetCacheValue(ErrorOr<LoginResponse> request)
        {
            return request.Value;
        }

        protected override ErrorOr<LoginResponse> GetCacheReturnType(object value)
        {
            return (LoginResponse)value;
        }
    }

    /*
    /// <summary>
    /// Invalidates the cached list of users (GetUsers Request) whenever a User is Added
    /// </summary>
    public class AddUserGetUsersCacheInvalidator : CacheInvalidator<AddUser, GetUsers, List<User>>
    {
        public AddUserGetUsersCacheInvalidator(ICache<GetUsers, List<User>> cache) : base(cache)
        {
        }

        protected override string GetCacheKeyIdentifier(AddUser request)
        {
            // invalidates entire list so no extra cache key / criteria is sent
            return string.Empty;
        }
    }

    /// <summary>
    /// Invalidates the cached list of users (GetUsers Request) whenever a User is Updated
    /// </summary>
    public class UpdateUserGetUsersCacheInvalidator : CacheInvalidator<UpdateUser, GetUsers, List<User>>
    {
        public UpdateUserGetUsersCacheInvalidator(ICache<GetUsers, List<User>> cache) : base(cache)
        {
        }

        protected override string GetCacheKeyIdentifier(UpdateUser request)
        {
            // invalidates entire list so no extra cache key / criteria is sent
            return string.Empty;
        }
    }

    /// <summary>
    /// Invalidates the cached list of users (GetUsers Request) whenever a User is Deleted
    /// </summary>
    public class DeleteUserGetUsersCacheInvalidator : CacheInvalidator<DeleteUser, GetUsers, List<User>>
    {
        public DeleteUserGetUsersCacheInvalidator(ICache<GetUsers, List<User>> cache) : base(cache)
        {
        }

        protected override string GetCacheKeyIdentifier(DeleteUser request)
        {
            // invalidates entire list so no extra cache key / criteria is sent
            return string.Empty;
        }
    }
    */



    /// <summary>
    /// Örnekleme için oluþturulmuþtur.
    /// </summary>
    public class RegisterCommandLoginQueryCacheInvalidator : CacheInvalidator<RegisterCommand, LoginQuery, ErrorOr<LoginResponse>>
    {
        public RegisterCommandLoginQueryCacheInvalidator(ICache<LoginQuery, ErrorOr<LoginResponse>> cache) : base(cache)
        {
        }

        protected override string GetCacheKeyIdentifier(RegisterCommand request)
        {
            return request.Email.ToString();
        }
    }


    //public class UpdateUserLoginQueryCacheInvalidator : CacheInvalidator<RegisterCommand, LoginQuery, ErrorOr<LoginResponse>>
    //{
    //    public UpdateUserLoginQueryCacheInvalidator(ICache<LoginQuery, ErrorOr<LoginResponse>> cache) : base(cache)
    //    {
    //    }

    //    protected override string GetCacheKeyIdentifier(RegisterCommand request)
    //    {
    //        return request.Email.ToString();
    //    }
    //}
}