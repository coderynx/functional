using Coderynx.Functional.Option;
using Microsoft.AspNetCore.Http;

namespace Coderynx.Functional.WebApi;

public static class OptionExtensions
{
    public static IResult ToHttpResult<T>(this ValueOption<T> option) where T : struct
    {
        return option.Match(
            some: Results.Ok,
            none: Results.NoContent
        );
    }

    public static IResult ToHttpResult<T>(this ValueOption<T> option, Func<T, object> transform) where T : struct
    {
        return option.Match(
            some: value => Results.Ok(transform(value)),
            none: Results.NoContent
        );
    }

    public static IResult ToHttpResult<T>(this Option<T> option) where T : class
    {
        return option.Match(
            some: Results.Ok,
            none: Results.NoContent
        );
    }

    public static IResult ToHttpResult<T>(this Option<T> option, Func<T, object> transform) where T : class
    {
        return option.Match(
            some: value => Results.Ok(transform(value)),
            none: Results.NoContent
        );
    }
}