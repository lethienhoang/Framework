using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Framework.Domain;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Framework.Middlewares
{
    public class ApiExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ApiExceptionHandlerMiddleware(RequestDelegate next, ILogger<ApiExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(DomainException domainException)
            {
                var error = new DomainExceptionContract()
                {
                    Key = domainException.Key,
                    Message = domainException.Message
                };
                _logger.LogError(domainException, domainException.Message);
                await WriteErrorMessageToResponse(context, HttpStatusCode.BadRequest, error);
            }
            catch (ForbiddenException)
            {
                var error = new DomainExceptionContract()
                {
                    Key = "Forbidden",
                    Message = "Forbidden"
                };

                await WriteErrorMessageToResponse(context, HttpStatusCode.Forbidden, error);
            }
            catch (Exception ex)
            {
                var error = new DomainExceptionContract()
                {
                    Key = "InternalServerError",
                    Message = ex.Message
                };

                _logger.LogError(ex, ex.Message, new object[] { });

                await WriteErrorMessageToResponse(context, HttpStatusCode.InternalServerError, error);
            }
        }

        private async Task WriteErrorMessageToResponse(HttpContext context, HttpStatusCode httpStatusCode, DomainExceptionContract error)
        {
            context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
            context.Response.Headers[HeaderNames.Pragma] = "no-cache";
            context.Response.Headers[HeaderNames.Expires] = "-1";
            context.Response.Headers.Remove(HeaderNames.ETag);

            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;
            await context.Response.WriteAsync(result);
        }
    }
}
