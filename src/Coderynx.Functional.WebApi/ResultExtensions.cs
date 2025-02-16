using Coderynx.Functional.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coderynx.Functional.WebApi;

public static class ResultExtensions
{
    public static IResult ToHttpResult(this Result.Result result)
    {
        return ToHttpResultInternal(result, null);
    }

    public static IResult ToHttpResult<TValue>(this Result<TValue> result)
    {
        return result.HasValue
            ? ToHttpResultInternal(result, result.Value)
            : ToHttpResultInternal(result, null);
    }

    public static IResult ToHttpResult<TValue>(this Result<TValue> result, Func<TValue, object> transform)
    {
        if (!result.HasValue)
        {
            return ToHttpResultInternal(result, null);
        }

        var transformResult = transform(result.Value);

        return ToHttpResultInternal(result, transformResult);
    }

    private static IResult ToHttpResultInternal(Result.Result result, object? value)
    {
        if (result.IsSuccess)
        {
            return result.SuccessType switch
            {
                ResultSuccess.Created => Results.Ok(value),
                ResultSuccess.Updated => Results.NoContent(),
                ResultSuccess.Deleted => Results.NoContent(),
                ResultSuccess.Found => Results.Ok(value),
                ResultSuccess.Accepted => Results.Accepted(),
                _ => Results.Ok()
            };
        }

        return result.ToProblem();
    }

    private static IResult ToProblem(this Result.Result result)
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