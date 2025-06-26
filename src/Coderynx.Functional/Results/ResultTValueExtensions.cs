using Coderynx.Functional.Results.Errors;
using Coderynx.Functional.Results.Successes;

namespace Coderynx.Functional.Results;

public static class ResultTValueExtensions
{
    public static TOutput Match<TOutput, TValue>(
        this Result<TValue> result,
        Func<TValue, TOutput> success,
        Func<Error, TOutput> error)
    {
        return result.IsFailure
            ? error(result.Error)
            : success(result.Value);
    }

    public static async Task<TOutput> MatchAsync<TOutput, TValue>(
        this Result<TValue> result,
        Func<TValue, Task<TOutput>> success,
        Func<Error, Task<TOutput>> error)
    {
        return result.IsFailure
            ? await error(result.Error)
            : await success(result.Value);
    }

    public static async Task<TOutput> MatchAsync<TOutput, TValue>(
        this Task<Result<TValue>> resultTask,
        Func<TValue, Task<TOutput>> success,
        Func<Error, Task<TOutput>> error)
    {
        var result = await resultTask;
        return result.IsFailure
            ? await error(result.Error)
            : await success(result.Value);
    }

    public static Result<TValue> Filter<TValue>(
        this Result<TValue> result,
        Func<Error, Error> predicate)
    {
        return result.IsSuccess
            ? result
            : new Result<TValue>(predicate(result.Error));
    }

    public static async Task<Result<TValue>> FilterAsync<TValue>(
        this Result<TValue> result,
        Func<Error, Task<Error>> predicate)
    {
        return result.IsSuccess
            ? result
            : new Result<TValue>(await predicate(result.Error));
    }

    public static async Task<Result<TValue>> FilterAsync<TValue>(
        this Task<Result<TValue>> resultTask,
        Func<Error, Task<Error>> predicate)
    {
        var result = await resultTask;
        return result.IsSuccess
            ? result
            : new Result<TValue>(await predicate(result.Error));
    }

    public static Result<TValue> Flatten<TValue>(this Result<Result<TValue>> result)
    {
        return result.IsSuccess ? result.Value : new Result<TValue>(result.Error);
    }

    public static async Task<Result<TValue>> FlattenAsync<TValue>(
        this Task<Result<Result<TValue>>> resultTask)
    {
        var result = await resultTask;
        return result.IsSuccess ? result.Value : new Result<TValue>(result.Error);
    }

    public static Result<TNext> Map<TNext, TValue>(this Result<TValue> result, Func<TValue, TNext> map)
    {
        return result.IsSuccess
            ? Success.Created(map(result.Value))
            : new Result<TNext>(result.Error);
    }

    public static async Task<Result<TNext>> MapAsync<TNext, TValue>(
        this Result<TValue> result,
        Func<TValue, Task<TNext>> map)
    {
        return result.IsSuccess
            ? new Result<TNext>(new Success<TNext>(result.Success.Kind, await map(result.Value)))
            : new Result<TNext>(result.Error);
    }

    public static Result<TNext> Bind<TNext, TValue>(this Result<TValue> result, Func<TValue, Result<TNext>> bind)
    {
        return result.IsSuccess ? bind(result.Value) : new Result<TNext>(result.Error);
    }

    public static async Task<Result<TNext>> BindAsync<TNext, TValue>(
        this Result<TValue> result,
        Func<TValue, Task<Result<TNext>>> bind)
    {
        return result.IsSuccess ? await bind(result.Value) : new Result<TNext>(result.Error);
    }

    public static async Task<Result<TNext>> BindAsync<TNext, TValue>(
        this Task<Result<TValue>> resultTask,
        Func<TValue, Task<Result<TNext>>> bind)
    {
        var result = await resultTask;
        return result.IsSuccess ? await bind(result.Value) : new Result<TNext>(result.Error);
    }

    public static Result<TValue> Tap<TValue>(this Result<TValue> result, Action<TValue> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value);
        }

        return result;
    }

    public static async Task<Result<TValue>> TapAsync<TValue>(
        this Result<TValue> result,
        Func<TValue, Task> action)
    {
        if (result.IsSuccess)
        {
            await action(result.Value);
        }

        return result;
    }

    public static async Task<Result<TValue>> TapAsync<TValue>(
        this Task<Result<TValue>> resultTask,
        Func<TValue, Task> action)
    {
        var result = await resultTask;
        if (result.IsSuccess)
        {
            await action(result.Value);
        }

        return result;
    }

    public static async Task<Result<TValue>> TapAsync<TValue>(
        this Result<TValue> result,
        Func<Task> action)
    {
        if (result.IsSuccess)
        {
            await action();
        }

        return result;
    }

    public static async Task<Result<TValue>> TapAsync<TValue>(
        this Task<Result<TValue>> resultTask,
        Func<Task> action)
    {
        var result = await resultTask;
        if (result.IsSuccess)
        {
            await action();
        }

        return result;
    }

    public static Result<TValue> Ensure<TValue>(
        this Result<TValue> result,
        Func<TValue, bool> predicate,
        Func<Error> onFailure)
    {
        if (result.IsFailure)
        {
            return result;
        }

        return predicate(result.Value)
            ? result
            : new Result<TValue>(onFailure());
    }

    public static async Task<Result<TValue>> EnsureAsync<TValue>(
        this Result<TValue> result,
        Func<TValue, Task<bool>> predicate,
        Func<Error> onFailure)
    {
        if (result.IsFailure)
        {
            return result;
        }

        return await predicate(result.Value)
            ? result
            : new Result<TValue>(onFailure());
    }

    public static async Task<Result<TValue>> EnsureAsync<TValue>(
        this Task<Result<TValue>> resultTask,
        Func<TValue, Task<bool>> predicate,
        Func<Error> onFailure)
    {
        var result = await resultTask;
        if (result.IsFailure)
        {
            return result;
        }

        return await predicate(result.Value)
            ? result
            : new Result<TValue>(onFailure());
    }
}