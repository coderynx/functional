using Microsoft.AspNetCore.Http;

namespace Coderynx.Result.WebApi;

public static class ResultExtensions
{
    public static IResult ToHttpResult(this Result result)
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
        if (!result.HasValue) return ToHttpResultInternal(result, null);

        var transformResult = transform(result.Value);

        return ToHttpResultInternal(result, transformResult);
    }

    private static IResult ToHttpResultInternal(Result result, object? value)
    {
        if (result.IsSuccess)
            return result.SuccessType switch
            {
                ResultSuccess.Created => Results.Ok(value),
                ResultSuccess.Updated => Results.NoContent(),
                ResultSuccess.Deleted => Results.NoContent(),
                ResultSuccess.Found => Results.Ok(value),
                ResultSuccess.Accepted => Results.Accepted(),
                _ => Results.Ok()
            };

        return result.Error.ResultError switch
        {
            ResultError.None => Results.BadRequest(result.Error.Code),
            ResultError.NotFound => Results.NotFound(result.Error.Code),
            ResultError.Conflict => Results.Conflict(result.Error.Code),
            ResultError.InvalidInput => Results.BadRequest(result.Error.Code),
            ResultError.Custom => Results.Problem(result.Error.Code),
            _ => Results.Problem(result.Error.Code)
        };
    }
}