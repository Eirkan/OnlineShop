﻿using MediatR;

namespace Product.Core.Domain.Messaging
{
    public interface ICache<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Get(TRequest request);
        Task Set(TRequest request, TResponse value);
        Task Remove(string cacheKeyIdentifier);
    }
}