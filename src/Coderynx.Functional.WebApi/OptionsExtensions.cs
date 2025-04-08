using Coderynx.Functional.Option;
using Microsoft.AspNetCore.Http;

namespace Coderynx.Functional.WebApi;

/// <summary>
/// Provides extension methods for converting Option and ValueOption types to HTTP results.
/// </summary>
public static class OptionExtensions
{
    /// <summary>
    /// Converts a <see cref="ValueOption{T}"/> to an HTTP result.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the ValueOption.</typeparam>
    /// <param name="option">The ValueOption to convert.</param>
    /// <returns>
    /// An HTTP 200 OK result if the ValueOption contains a value, or an HTTP 204 No Content result if it does not.
    /// </returns>
    public static IResult ToHttpResult<T>(this ValueOption<T> option) where T : struct
    {
        return option.Match(
            some: Results.Ok,
            none: Results.NoContent
        );
    }

    /// <summary>
    /// Converts a <see cref="ValueOption{T}"/> to an HTTP result, applying a transformation to the value if present.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the ValueOption.</typeparam>
    /// <param name="option">The ValueOption to convert.</param>
    /// <param name="transform">A function to transform the value if the ValueOption contains one.</param>
    /// <returns>
    /// An HTTP 200 OK result with the transformed value if the ValueOption contains a value, 
    /// or an HTTP 204 No Content result if it does not.
    /// </returns>
    public static IResult ToHttpResult<T>(this ValueOption<T> option, Func<T, object> transform) where T : struct
    {
        return option.Match(
            some: value => Results.Ok(transform(value)),
            none: Results.NoContent
        );
    }

    /// <summary>
    /// Converts an <see cref="Option{T}"/> to an HTTP result.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the Option.</typeparam>
    /// <param name="option">The Option to convert.</param>
    /// <returns>
    /// An HTTP 200 OK result if the Option contains a value, or an HTTP 204 No Content result if it does not.
    /// </returns>
    public static IResult ToHttpResult<T>(this Option<T> option) where T : class
    {
        return option.Match(
            some: Results.Ok,
            none: Results.NoContent
        );
    }

    /// <summary>
    /// Converts an <see cref="Option{T}"/> to an HTTP result, applying a transformation to the value if present.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the Option.</typeparam>
    /// <param name="option">The Option to convert.</param>
    /// <param name="transform">A function to transform the value if the Option contains one.</param>
    /// <returns>
    /// An HTTP 200 OK result with the transformed value if the Option contains a value, 
    /// or an HTTP 204 No Content result if it does not.
    /// </returns>
    public static IResult ToHttpResult<T>(this Option<T> option, Func<T, object> transform) where T : class
    {
        return option.Match(
            some: value => Results.Ok(transform(value)),
            none: Results.NoContent
        );
    }
}