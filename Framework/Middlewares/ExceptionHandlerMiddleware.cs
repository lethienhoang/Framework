using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Framework.Domain;

namespace Framework.Middlewares
{
    public static class ExceptionHandlerMiddlewareExceptions
    {
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            builder.Run(async context =>
            {
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = errorFeature.Error;
                if (exception is DomainException)
                {
                    var domainException = exception as DomainException;
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";
                    var error = new DomainExceptionContract()
                    {
                        Key = domainException.Key,
                        Message = domainException.Message
                    };
                    await context.Response.WriteAsync(error.ToString());
                }
                else if (exception is ForbiddenException)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    var error = new DomainExceptionContract()
                    {
                        Key = "Forbidden",
                        Message = "Forbidden"
                    };
                    await context.Response.WriteAsync(error.ToString());
                }
                else
                {
                    var loggerFactory = builder.ApplicationServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
                    var exceptionLogger = loggerFactory.CreateLogger(typeof(ExceptionHandlerMiddlewareExceptions));
                    exceptionLogger.LogError(exception, "Exception");

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            });
        }
    }
}
