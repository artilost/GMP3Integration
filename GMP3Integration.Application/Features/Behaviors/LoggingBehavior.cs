using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Application.Features.Behaviors
{
    public class LoggingBehavior <TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("MediatR handling {RequestType}", typeof(TRequest).Name);

            try
            {
                var response = await next();
                sw.Stop();
                _logger.LogInformation("MediatR handled {RequestType} in {Elapsed} ms", typeof(TRequest).Name, sw.ElapsedMilliseconds);
                return response;
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "MediatR error on {RequestType} after {Elapsed} ms", typeof(TRequest).Name, sw.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
