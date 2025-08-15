using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GMP3Integration.API.Middlewares
{
    public sealed class CorrelationIdMiddleware
    {
        public const string HeaderName = "X-Correlation-ID";
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string corrId = context.Request.Headers.ContainsKey(HeaderName)
                ? context.Request.Headers[HeaderName].ToString()
                : Guid.NewGuid().ToString("N");

            context.Response.Headers[HeaderName] = corrId;

            using (_logger.BeginScope(new Dictionary<string, object> { { "correlationId", corrId } }))
            {
                _logger.LogInformation("Incoming {Method} {Path}", context.Request.Method, context.Request.Path);
                await _next(context);
            }
        }
    }
}
