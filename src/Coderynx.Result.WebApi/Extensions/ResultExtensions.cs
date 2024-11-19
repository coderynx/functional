using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coderynx.Result.WebApi.Extensions;

internal static class ResultExtensions
{
    internal static IResult ToProblem(this Result result)
    {
        var details = new ProblemDetails
        {
            Type = result.Error.ResultError switch
            {
                ResultError.None => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ResultError.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                ResultError.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                _ => "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            },
            Title = result.Error.Code,
            Status = result.Error.ResultError switch
            {
                ResultError.None => StatusCodes.Status400BadRequest,
                ResultError.NotFound => StatusCodes.Status404NotFound,
                ResultError.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            },
            Detail = result.Error.Message,
            Instance = null
        };

        return Results.Problem(details);
    }
}