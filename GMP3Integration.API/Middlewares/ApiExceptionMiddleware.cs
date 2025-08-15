using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace GMP3Integration.API.Middlewares
{
    public sealed class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ApiExceptionMiddleware(RequestDelegate next)
        {
            _next = next;            
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ArgumentException ex)
            {
                await WriteProblem(httpContext, (int)HttpStatusCode.BadRequest,
                    title : "Geçersiz istek verisi",
                    detail : ex.Message
                    );
            }
            catch (Exception ex)
            {
                await WriteProblem(httpContext, (int)HttpStatusCode.InternalServerError,
                   title: "Beklenmeyen bir hata oluştu",
                   detail: ex.Message);
            }
        }
        private static async Task WriteProblem(HttpContext httpContext, int statusCode, string title, string detail)
        {
         httpContext.Response.ContentType = "application/problem+json"; 
            httpContext.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request?.Path.Value
            };

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            var json = JsonSerializer.Serialize(problemDetails);
            await httpContext.Response.WriteAsync(json);
        }
    }
}
