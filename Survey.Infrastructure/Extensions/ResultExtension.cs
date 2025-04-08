using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.Infrastructure.Abstractions;

namespace Survey.Infrastructure.Extensions
{
    public static class ResultExtension
    {
        public static ObjectResult StandareError(this Result result, int statusCodes)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException("Result is not successful");

            var problems = Results.Problem(statusCode: statusCodes);
            var problemDetails = problems.GetType().GetProperty(nameof(ProblemDetails))?.GetValue(problems) as ProblemDetails;

            problemDetails!.Extensions = new Dictionary<string, object?>
                {
                    {
                        "errors", new[] {result.Error }
                    }
            };

            return new ObjectResult(problemDetails);
        }

    }
}
