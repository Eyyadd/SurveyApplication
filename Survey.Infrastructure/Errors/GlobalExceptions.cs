using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.Errors
{
    public class GlobalExceptions(ILogger<GlobalExceptions> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptions> _Logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _Logger.LogError(exception.Message);

            var problemDetails = new ProblemDetails()
            {
                //Detail = exception.Message,
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-500-internal-server-error",
                //Instance = httpContext.Request.Path,
                Status = StatusCodes.Status500InternalServerError,
                Title = "An exception occurred while processing your request.",
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

            return true;
        }
    }
}
