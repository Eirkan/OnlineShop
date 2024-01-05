using MediatR;
using Microsoft.AspNetCore.Http;
using Product.Core.Domain.Extensions;
using System.Transactions;

namespace Product.Application.Common.Behaviours
{
    public sealed class TransactionBehaviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly TransactionOptions _transactionOptions;
        private readonly HttpContext _httpContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionBehavior{TRequest,TResponse}"/> class.
        /// </summary>
        public TransactionBehaviour(IHttpContextAccessor httpContextAccessor)
        {
            _transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };
            _httpContext = httpContextAccessor.HttpContext!;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request.IsQuery())
            {
                return await next();
            }

            using var transactionScope = new TransactionScope(TransactionScopeOption.Required, _transactionOptions, TransactionScopeAsyncFlowOption.Enabled);
            var transactionId = Transaction.Current?.TransactionInformation.LocalIdentifier?.Split(":")[0];

            TResponse response = await next();

            transactionScope.Complete();



            return response;
        }
    }
}