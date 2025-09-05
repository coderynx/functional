using Coderynx.Functional.Results;
using Coderynx.Functional.Results.Errors;
using Coderynx.Functional.Results.Successes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coderynx.Functional.WebApi;

/// <summary>
///     Provides extension methods for converting functional Result objects to ASP.NET Core IResult responses.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    ///     Converts a Result to an appropriate HTTP result.
    /// </summary>
    /// <param name="result">The Result to convert.</param>
    /// <returns>An IResult representing the appropriate HTTP response based on the Result state.</returns>
    public static IResult ToHttpResult(this Result result)
    {
        return ToHttpResultInternal(result, null);
    }

    /// <summary>
    ///     Converts a Result{TValue} to an appropriate HTTP result, including the contained value if successful.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the Result.</typeparam>
    /// <param name="result">The Result{TValue} to convert.</param>
    /// <returns>An IResult representing the appropriate HTTP response based on the Result state.</returns>
    public static IResult ToHttpResult<TValue>(this Result<TValue> result)
    {
        return result.Success.Value is not null
            ? ToHttpResultInternal(result, result.Value)
            : ToHttpResultInternal(result, null);
    }

    /// <summary>
    ///     Converts a Result{TValue} to an appropriate HTTP result using a transform function for the value.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the Result.</typeparam>
    /// <param name="result">The Result{TValue} to convert.</param>
    /// <param name="transform">Function to transform the result value to an appropriate response object.</param>
    /// <returns>An IResult representing the appropriate HTTP response based on the Result state.</returns>
    public static IResult ToHttpResult<TValue>(this Result<TValue> result, Func<TValue, object> transform)
    {
        if (result.Success.Value is null)
        {
            return ToHttpResultInternal(result, null);
        }

        var transformerResult = transform(result.Value);
        return ToHttpResultInternal(result, transformerResult);
    }

    /// <summary>
    ///     Internal helper method to convert a Result to an HTTP response.
    /// </summary>
    /// <param name="result">The Result to convert.</param>
    /// <param name="value">The value to include in the response (if applicable).</param>
    /// <returns>An appropriate IResult based on the Result's Success or Error state.</returns>
    private static IResult ToHttpResultInternal(Result result, object? value)
    {
        if (result.IsSuccess)
        {
            return result.Success.Kind switch
            {
                SuccessKind.Created => Microsoft.AspNetCore.Http.Results.Ok(value),
                SuccessKind.Updated => Microsoft.AspNetCore.Http.Results.NoContent(),
                SuccessKind.Deleted => Microsoft.AspNetCore.Http.Results.NoContent(),
                SuccessKind.Found => Microsoft.AspNetCore.Http.Results.Ok(value),
                SuccessKind.Accepted => Microsoft.AspNetCore.Http.Results.Accepted(),
                _ => Microsoft.AspNetCore.Http.Results.Ok()
            };
        }

        return result.ToProblem();
    }

    /// <summary>
    ///     Converts a failed Result to a ProblemDetails HTTP response.
    /// </summary>
    /// <param name="result">The Result containing an error.</param>
    /// <returns>An IResult representing a problem details response based on the error information.</returns>
    private static IResult ToProblem(this Result result)
    {
        var details = new ProblemDetails
        {
            Type = result.Error.Kind switch
            {
                ErrorKind.None => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorKind.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                ErrorKind.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                ErrorKind.InvalidInput => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                _ => "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            },
            Title = result.Error.Code,
            Status = result.Error.Kind switch
            {
                ErrorKind.None => StatusCodes.Status400BadRequest,
                ErrorKind.NotFound => StatusCodes.Status404NotFound,
                ErrorKind.Conflict => StatusCodes.Status409Conflict,
                ErrorKind.InvalidInput => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            },
            Detail = result.Error.Message,
            Instance = null
        };

        return Microsoft.AspNetCore.Http.Results.Problem(details);
    }
}