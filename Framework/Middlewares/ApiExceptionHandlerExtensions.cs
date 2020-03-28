using Microsoft.AspNetCore.Builder;

namespace Framework.Middlewares
{
    public static class ApiExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder app) => app.UseMiddleware<ApiExceptionHandlerMiddleware>();
    }
}
