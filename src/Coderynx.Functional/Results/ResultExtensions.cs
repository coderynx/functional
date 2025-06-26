using Coderynx.Functional.Results.Errors;

namespace Coderynx.Functional.Results;

public static class ResultExtensions
{
    public static TOutput Match<TOutput>(this Result result, Func<TOutput> success, Func<Error, TOutput> error)
    {
        return result.IsFailure
            ? error(result.Error)
            : success();
    }

    public static async Task<TOutput> MatchAsync<TOutput>(
        this Result result,
        Func<Task<TOutput>> success,
        Func<Error, Task<TOutput>> error)
    {
        return result.IsFailure
            ? await error(result.Error)
            : await success();
    }
    
    public static async Task<TOutput> MatchAsync<TOutput>(
        this Task<Result> resultTask,
        Func<Task<TOutput>> success,
        Func<Error, Task<TOutput>> error)
    {
        var result = await resultTask;
        return result.IsFailure
            ? await error(result.Error)
            : await success();
    }

    public static Result Bind(this Result result, Func<Result> bind)
    {
        return result.IsSuccess ? bind() : result;
    }

    public static async Task<Result> BindAsync(this Result result, Func<Task<Result>> bind)
    {
        return result.IsSuccess ? await bind() : result;
    }
    
    public static async Task<Result> BindAsync(this Task<Result> result, Func<Task<Result>> bind)
    {
        var res = await result;
        return res.IsSuccess ? await bind() : res;
    }

    public static Result Tap(this Result result, Action action)
    {
        if (result.IsSuccess)
        {
            action();
        }

        return result;
    }

    public static async Task<Result> TapAsync(this Result result, Func<Task> action)
    {
        if (result.IsSuccess)
        {
            await action();
        }

        return result;
    }

    public static async Task<Result> TapAsync(this Task<Result> resultTask, Func<Task> action)
    {
        var result = await resultTask;
        if (result.IsSuccess)
        {
            await action();
        }

        return result;
    }
    
    public static Result Then(this Result result, Func<Result> next)
    {
        return result.IsSuccess ? next() : result;
    }
    
    public static async Task<Result> ThenAsync(this Result result, Func<Task<Result>> next)
    {
        return result.IsSuccess ? await next() : result;
    }
    
    public static async Task<Result> ThenAsync(
        this Task<Result> task,
        Func<Task<Result>> next)
    {
        var result = await task;
        return result.IsSuccess ? await next() : result;
    }
}