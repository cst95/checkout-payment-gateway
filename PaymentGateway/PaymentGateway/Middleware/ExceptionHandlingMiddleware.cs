using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models;

namespace PaymentGateway.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError("Unhandled exception {Message} occured. Exception: {Exception}", exception.Message, exception);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                
                var response = new ApiException(context.Response.StatusCode, "Internal Server Error");
                var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

                await context.Response.WriteAsync(jsonResponse);
            } 
        }
    }
}