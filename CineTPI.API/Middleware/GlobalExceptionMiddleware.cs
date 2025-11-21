using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging; // Para loggear el error
using System;
using System.Net;
using System.Text.Json; 
using System.Threading.Tasks;

namespace CineTPI.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            catch (Exception ex)
            {

                _logger.LogError(ex, "Ocurrió una excepción no controlada: {Message}", ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500


                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Ocurrió un error interno en el servidor. Por favor, intente más tarde."

                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}