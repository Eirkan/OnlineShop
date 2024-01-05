using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Product.Application.Common.Behaviours
{
    public sealed class PerformanceMonitorBehaviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr

    {
        private readonly Stopwatch _stopwatch;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceMonitorBehaviour{TRequest,TResponse}"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public PerformanceMonitorBehaviour(ILogger<PerformanceMonitorBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _stopwatch.Start();

            TResponse response = await next();

            _stopwatch.Stop();

            string requestName = typeof(TRequest).Name;

            if (_stopwatch.ElapsedMilliseconds > 500)
            {
                // TODO: Send some kind of notification?
                _logger.LogWarning("Request {Name} completed in {}ms", requestName, _stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogInformation("Request {Name} completed in {}ms", requestName, _stopwatch.ElapsedMilliseconds);
            }

            return response;
        }
    }
}
