﻿using MediatR;

namespace Customer.Core.Domain.Messaging
{
    /// <summary>
    /// Represents the marker interface for a query.
    /// </summary>
    /// <typeparam name="TResult">The query result type.</typeparam>
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
