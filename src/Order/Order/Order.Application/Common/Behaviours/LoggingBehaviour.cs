using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Domain.Services;

namespace Order.Application.Common.Behaviours
{
    public sealed class LoggingBehaviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
    {
        private readonly ILogger _logger;
        private readonly IDateTimeProvider _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingBehaviour{TRequest,TResponse}"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="dateTime">The date time instance.</param>
        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger, IDateTimeProvider dateTime)
        {
            _logger = logger;
            _dateTime = dateTime;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;

            _logger.LogInformation("Handling request: {RequestName} at {Time}", requestName, _dateTime.UtcNow);

            TResponse response = await next();

            _logger.LogInformation("Finished handling request: {RequestName} at {Time}", requestName, _dateTime.UtcNow);

            return response;
        }

    }
}
