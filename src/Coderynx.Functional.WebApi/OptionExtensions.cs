using Coderynx.Functional.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HttpResults = Microsoft.AspNetCore.Http.Results;

namespace Coderynx.Functional.WebApi;

/// <summary>
/// Provides extension methods for converting <see cref="Option{T}"/> and <see cref="ValueOption{T}"/> 
/// instances to HTTP results.
/// </summary>
public static class OptionExtensions
{
    /// <summary>
    /// Converts a <see cref="ValueOption{T}"/> to an <see cref="IResult"/> HTTP response.
    /// Returns OK (200) with the value if the option has a value, or a 404 Not Found if none.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the option. Must be a value type.</typeparam>
    /// <param name="option">The option to convert.</param>
    /// <returns>An <see cref="IResult"/> representing either a 200 OK or 404 Not Found response.</returns>
    public static IResult ToHttpResult<T>(this ValueOption<T> option) where T : struct
    {
        return option.Match(
            some: HttpResults.Ok,
            none: () => HttpResults.Problem(NotFound)
        );
    }

    /// <summary>
    /// Converts a <see cref="ValueOption{T}"/> to an <see cref="IResult"/> HTTP response,
    /// applying a transformation function to the value if present.
    /// Returns OK (200) with the transformed value if the option has a value, or a 404 Not Found if none.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the option. Must be a value type.</typeparam>
    /// <param name="option">The option to convert.</param>
    /// <param name="transform">A function to transform the value before returning it.</param>
    /// <returns>An <see cref="IResult"/> representing either a 200 OK with the transformed value or 404 Not Found response.</returns>
    public static IResult ToHttpResult<T>(this ValueOption<T> option, Func<T, object> transform) where T : struct
    {
        return option.Match(
            some: value => HttpResults.Ok(transform(value)),
            none: () => HttpResults.Problem(NotFound)
        );
    }

    /// <summary>
    /// Converts an <see cref="Option{T}"/> to an <see cref="IResult"/> HTTP response.
    /// Returns OK (200) with the value if the option has a value, or a 404 Not Found if none.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the option. Must be a reference type.</typeparam>
    /// <param name="option">The option to convert.</param>
    /// <returns>An <see cref="IResult"/> representing either a 200 OK or 404 Not Found response.</returns>
    public static IResult ToHttpResult<T>(this Option<T> option) where T : class
    {
        return option.Match(
            some: HttpResults.Ok,
            none: () => HttpResults.Problem(NotFound)
        );
    }

    /// <summary>
    /// Converts an <see cref="Option{T}"/> to an <see cref="IResult"/> HTTP response,
    /// applying a transformation function to the value if present.
    /// Returns OK (200) with the transformed value if the option has a value, or a 404 Not Found if none.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the option. Must be a reference type.</typeparam>
    /// <param name="option">The option to convert.</param>
    /// <param name="transform">A function to transform the value before returning it.</param>
    /// <returns>An <see cref="IResult"/> representing either a 200 OK with the transformed value or 404 Not Found response.</returns>
    public static IResult ToHttpResult<T>(this Option<T> option, Func<T, object> transform) where T : class
    {
        return option.Match(
            some: value => HttpResults.Ok(transform(value)),
            none: () => HttpResults.Problem(NotFound)
        );
    }

    /// <summary>
    /// Standard problem details for a 404 Not Found response.
    /// </summary>
    private static readonly ProblemDetails NotFound = new()
    {
        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        Title = "Not Found",
        Status = StatusCodes.Status404NotFound,
        Detail = "Resource not found.",
        Instance = null
    };
}