
using Microsoft.AspNetCore.Mvc;

namespace Survey.API.Helper
{
    public class HandleExceptions(RequestDelegate next,ILogger<HandleExceptions> logger)
    {
        private readonly RequestDelegate _Next = next;
        private readonly ILogger<HandleExceptions> _Logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
			try
			{
				await _Next(context);
            }
			catch (Exception ex)
			{
                _Logger.LogError(ex.Message);

                var problemDetails = new ProblemDetails
                {
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-500-internal-server-error",
                    Title = "An Exception error occurred",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = context.Request.Path,
                    
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(problemDetails);

            }
        }
    }
}
