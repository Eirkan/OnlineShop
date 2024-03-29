﻿using MediatR;
using Order.Core.Domain.Extensions;
using Order.Core.Domain.UnitofWork;

namespace Order.Application.Common.Behaviours
{
    /// <summary>
    /// Represents a unit of work behavior that wraps a request and manages the unit of work.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public sealed class UnitOfWorkBehaviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkBehaviour{TRequest,TResponse}"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance.</param>
        /// <param name="session">The async document session instance.</param>
        public UnitOfWorkBehaviour(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response = await next();

            if (request.IsQuery())
            {
                return response;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
