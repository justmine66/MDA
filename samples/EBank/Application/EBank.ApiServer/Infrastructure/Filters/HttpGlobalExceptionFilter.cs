using EBank.ApiServer.Infrastructure.ActionResults;
using EBank.ApiServer.Infrastructure.Exceptions;
using EBank.ApiServer.Models.Output;
using MDA.Infrastructure.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EBank.ApiServer.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is DomainException domainException)
            {
                var aggregateMessage = domainException.Messages == null
                    ? context.Exception.Message
                    : JsonConvert.SerializeObject(domainException.Messages);

                _logger.LogError(new EventId(context.Exception.HResult),
                    context.Exception,
                    aggregateMessage);

                var result = ApiErrorResult.BadRequest(domainException.Messages ?? new[] { context.Exception.Message });

                context.Result = new BadRequestObjectResult(result);
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                _logger.LogError(new EventId(context.Exception.HResult),
                    context.Exception,
                    context.Exception.Message);

                object devMessage = null;
                if (_env.IsDevelopment())
                {
                    devMessage = LogFormatter.PrintException(context.Exception);
                }

                var result = ApiExceptionResult.InternalServerError(new[] { "An server error occur." }, devMessage);

                context.Result = new InternalServerErrorObjectResult(result);
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            context.ExceptionHandled = true;
        }
    }
}
