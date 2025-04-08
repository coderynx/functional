using Coderynx.Functional.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coderynx.Functional.WebApi;

/// <summary>
/// Provides extension methods for converting `Result` objects to HTTP results.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts a `Result` object to an HTTP result.
    /// </summary>
    /// <param name="result">The `Result` object to convert.</param>
    /// <returns>An `IResult` representing the HTTP response.</returns>
    public static IResult ToHttpResult(this Result.Result result)
    {
        return ToHttpResultInternal(result, null);
    }

    /// <summary>
    /// Converts a `Result` object with a value to an HTTP result.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the `Result`.</typeparam>
    /// <param name="result">The `Result` object to convert.</param>
    /// <returns>An `IResult` representing the HTTP response.</returns>
    public static IResult ToHttpResult<TValue>(this Result<TValue> result)
    {
        return result.HasValue
            ? ToHttpResultInternal(result, result.Value)
            : ToHttpResultInternal(result, null);
    }

    /// <summary>
    /// Converts a `Result` object with a value to an HTTP result, applying a transformation to the value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the `Result`.</typeparam>
    /// <param name="result">The `Result` object to convert.</param>
    /// <param name="transform">A function to transform the value before converting to an HTTP result.</param>
    /// <returns>An `IResult` representing the HTTP response.</returns>
    public static IResult ToHttpResult<TValue>(this Result<TValue> result, Func<TValue, object> transform)
    {
        if (!result.HasValue)
        {
            return ToHttpResultInternal(result, null);
        }

        var transformResult = transform(result.Value);

        return ToHttpResultInternal(result, transformResult);
    }

    /// <summary>
    /// Internal method to convert a `Result` object to an HTTP result.
    /// </summary>
    /// <param name="result">The `Result` object to convert.</param>
    /// <param name="value">The value to include in the HTTP response, if applicable.</param>
    /// <returns>An `IResult` representing the HTTP response.</returns>
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

    /// <summary>
    /// Converts a failed `Result` object to a `ProblemDetails` HTTP response.
    /// </summary>
    /// <param name="result">The failed `Result` object to convert.</param>
    /// <returns>An `IResult` representing the HTTP problem response.</returns>
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