using Coderynx.Functional.Results.Errors;
using Coderynx.Functional.Results.Successes;

namespace Coderynx.Functional.Results;

/// <summary>
///     Provides extension methods for working with <see cref="Result" /> objects, enabling fluent handling of success and
///     failure cases.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Transforms a <see cref="Result" /> into a new result containing a value of a different type, if the original result
    /// is successful. If the original result is a failure, the error is propagated instead.
    /// </summary>
    /// <typeparam name="TNext">The type of the value in the new result.</typeparam>
    /// <param name="result">The original <see cref="Result" /> to transform.</param>
    /// <param name="map">A function to generate the transformed value when the <paramref name="result" /> is successful.</param>
    /// <returns>A new <see cref="Result{TNext}" /> containing the transformed value, or the error from the original result.</returns>
    public static Result<TNext> Map<TNext>(this Result result, Func<TNext> map)
    {
        return result.IsSuccess
            ? new Success<TNext>(result.Success.Kind, map())
            : result.Error;
    }

    /// <summary>
    /// Asynchronously transforms a <see cref="Result" /> into a new result containing a value of a different type,
    /// if the original result is successful. If the original result is a failure, the error is propagated instead.
    /// </summary>
    /// <typeparam name="TNext">The type of the value in the new result.</typeparam>
    /// <param name="result">The task representing the original <see cref="Result" /> to transform.</param>
    /// <param name="map">A function to generate the transformed value when the <paramref name="result" /> is successful.</param>
    /// <returns>A task representing the new <see cref="Result{TNext}" /> containing the transformed value,
    /// or the error from the original result.</returns>
    public static async Task<Result<TNext>> MapAsync<TNext>(this Task<Result> result, Func<TNext> map)
    {
        var res = await result;
        return res.IsSuccess
            ? new Success<TNext>(res.Success.Kind, map())
            : res.Error;
    }
    
    /// <summary>
    ///     Executes one of the provided functions based on the success or failure state of the <see cref="Result" />.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output returned by the match functions.</typeparam>
    /// <param name="result">The <see cref="Result" /> instance to evaluate.</param>
    /// <param name="success">A function to execute if the result is successful.</param>
    /// <param name="error">A function to execute if the result is a failure, receiving the <see cref="Error" /> instance.</param>
    /// <returns>The output of the executed function, determined by the <see cref="Result" /> state.</returns>
    public static TOutput Match<TOutput>(this Result result, Func<TOutput> success, Func<Error, TOutput> error)
    {
        return result.IsFailure
            ? error(result.Error)
            : success();
    }

    /// <summary>
    ///     Asynchronously executes one of the provided functions based on the success or failure state of the
    ///     <see cref="Result" />.
    /// </summary>
    /// <typeparam name="TOutput">The type of the value returned by the asynchronous match functions.</typeparam>
    /// <param name="result">The <see cref="Result" /> instance to evaluate.</param>
    /// <param name="success">An asynchronous function to execute if the result is successful.</param>
    /// <param name="error">
    ///     An asynchronous function to execute if the result is a failure, receiving the <see cref="Error" />
    ///     instance.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the output of the executed
    ///     asynchronous function, determined by the state of the <see cref="Result" />.
    /// </returns>
    public static async Task<TOutput> MatchAsync<TOutput>(
        this Result result,
        Func<Task<TOutput>> success,
        Func<Error, Task<TOutput>> error)
    {
        return result.IsFailure
            ? await error(result.Error)
            : await success();
    }

    /// <summary>
    ///     Executes one of the provided asynchronous functions based on the success or failure state of the
    ///     <see cref="Result" /> returned from the given <see cref="Task" />.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output returned by the asynchronous match functions.</typeparam>
    /// <param name="resultTask">The <see cref="Task" /> that resolves to a <see cref="Result" /> to evaluate.</param>
    /// <param name="success">An asynchronous function to execute if the result represents a success.</param>
    /// <param name="error">
    ///     An asynchronous function to execute if the result represents a failure, receiving the
    ///     <see cref="Error" /> instance.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> resolving to the output of the executed asynchronous function, determined by the
    ///     <see cref="Result" /> state.
    /// </returns>
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

    /// <summary>
    ///     Executes the provided bind function if the <see cref="Result" /> instance represents a success.
    ///     Otherwise, returns the current <see cref="Result" /> without calling the bind function.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> instance to evaluate.</param>
    /// <param name="bind">A function to execute if the result is successful, returning a new <see cref="Result" />.</param>
    /// <returns>The result of the bind function if the original result is successful; otherwise, the original result.</returns>
    public static Result Bind(this Result result, Func<Result> bind)
    {
        return result.IsSuccess ? bind() : result;
    }

    /// <summary>
    /// Transforms the current <see cref="Result" /> into another <see cref="Result{TValue}" /> by applying the specified function
    /// if the current result represents a successful state. If the current result is a failure, the error is propagated.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the resulting <see cref="Result{TValue}" />.</typeparam>
    /// <param name="result">The current <see cref="Result" /> to be transformed.</param>
    /// <param name="func">A function that returns a <see cref="Result{TValue}" /> to be used when the current result is successful.</param>
    /// <returns>A new <see cref="Result{TValue}" /> either containing the transformed value if the operation is successful
    /// or propagating the error from the current result if it is a failure.</returns>
    public static Result<TValue> Bind<TValue>(this Result result, Func<Result<TValue>> func)
    {
        return result.IsSuccess
            ? func()
            : result.Error;
    }

    /// <summary>
    ///     Applies an asynchronous bind function to a successful <see cref="Result" /> to produce a new result.
    ///     If the original <see cref="Result" /> is a failure, the bind function will not be invoked, and the failure is
    ///     returned directly.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> to evaluate and bind if successful.</param>
    /// <param name="bind">
    ///     An asynchronous function that transforms a successful <see cref="Result" /> into a new
    ///     <see cref="Result" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> representing the result of the bind operation.
    ///     If the original <see cref="Result" /> is successful, the result of the bind function is returned; otherwise, the
    ///     failure is returned.
    /// </returns>
    public static async Task<Result> BindAsync(this Result result, Func<Task<Result>> bind)
    {
        return result.IsSuccess ? await bind() : result;
    }

    /// <summary>
    ///     Asynchronously binds a provided function to a <see cref="Result" /> encapsulated in a <see cref="Task{TResult}" />,
    ///     executing the function only if the result is successful.
    /// </summary>
    /// <param name="result">A task that resolves to a <see cref="Result" /> which determines the outcome.</param>
    /// <param name="bind">
    ///     A function that returns a task resolving to a <see cref="Result" /> to execute if the result is
    ///     successful.
    /// </param>
    /// <returns>
    ///     A task that resolves to the <see cref="Result" /> of the executed function if successful, or the original
    ///     failure result.
    /// </returns>
    public static async Task<Result> BindAsync(this Task<Result> result, Func<Task<Result>> bind)
    {
        var res = await result;
        return res.IsSuccess ? await bind() : res;
    }

    /// <summary>
    ///     Invokes the specified action if the <see cref="Result" /> is successful, and returns the same <see cref="Result" />
    ///     .
    /// </summary>
    /// <param name="result">The <see cref="Result" /> instance to evaluate.</param>
    /// <param name="action">The action to execute if the result is successful.</param>
    /// <returns>The original <see cref="Result" /> instance, allowing further method chaining.</returns>
    public static Result Tap(this Result result, Action action)
    {
        if (result.IsSuccess)
        {
            action();
        }

        return result;
    }

    /// <summary>
    ///     Executes the provided asynchronous function if the <see cref="Result" /> instance represents a success.
    ///     This allows additional operations or side effects to be performed without modifying the result state.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> instance to evaluate.</param>
    /// <param name="action">An asynchronous function to execute if the <see cref="Result" /> is successful.</param>
    /// <returns>The original <see cref="Result" /> instance, ensuring uninterrupted chaining of operations.</returns>
    public static async Task<Result> TapAsync(this Result result, Func<Task> action)
    {
        if (result.IsSuccess)
        {
            await action();
        }

        return result;
    }

    /// <summary>
    ///     Executes the provided asynchronous action if the <see cref="Result" /> is successful and returns the
    ///     <see cref="Result" />.
    /// </summary>
    /// <param name="resultTask">A task representing the <see cref="Result" /> to evaluate.</param>
    /// <param name="action">An asynchronous action to execute if the result is successful.</param>
    /// <returns>A task representing the original <see cref="Result" />, allowing further chaining.</returns>
    public static async Task<Result> TapAsync(this Task<Result> resultTask, Func<Task> action)
    {
        var result = await resultTask;
        if (result.IsSuccess)
        {
            await action();
        }

        return result;
    }

    /// <summary>
    ///     Executes the next operation in a fluent chain if the current <see cref="Result" /> is successful.
    /// </summary>
    /// <param name="result">The current <see cref="Result" /> instance to evaluate.</param>
    /// <param name="next">
    ///     A function that represents the next operation to execute if the current <see cref="Result" /> is
    ///     successful.
    /// </param>
    /// <returns>
    ///     The result of the next operation if the current <see cref="Result" /> is successful;
    ///     otherwise, the original failure <see cref="Result" />.
    /// </returns>
    public static Result Then(this Result result, Func<Result> next)
    {
        return result.IsSuccess ? next() : result;
    }

    /// <summary>
    ///     Executes the provided asynchronous function if the <see cref="Result" /> is successful,
    ///     or returns the current <see cref="Result" /> if it represents a failure.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> instance to evaluate.</param>
    /// <param name="next">An asynchronous function to execute if the <see cref="Result" /> is successful.</param>
    /// <returns>
    ///     A task representing the asynchronous operation.
    ///     The result contains either the output of the <paramref name="next" /> function or the original failure.
    /// </returns>
    public static async Task<Result> ThenAsync(this Result result, Func<Task<Result>> next)
    {
        return result.IsSuccess ? await next() : result;
    }

    /// <summary>
    ///     Executes the provided asynchronous function if the <see cref="Result" /> is successful; otherwise, returns the
    ///     original failure result.
    /// </summary>
    /// <param name="task">The task representing the asynchronous operation that resolves into a <see cref="Result" />.</param>
    /// <param name="next">
    ///     A function to execute if the resolved <see cref="Result" /> is successful; it returns a new
    ///     asynchronous <see cref="Result" />.
    /// </param>
    /// <returns>The result of the executed function if successful, or the original failure result.</returns>
    public static async Task<Result> ThenAsync(
        this Task<Result> task,
        Func<Task<Result>> next)
    {
        var result = await task;
        return result.IsSuccess ? await next() : result;
    }

    /// <summary>
    /// Converts the specified <see cref="Result" /> into a <see cref="Task{TResult}" />, wrapping it in an asynchronous representation.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> instance to be wrapped as a task.</param>
    /// <returns>A completed <see cref="Task{TResult}" /> containing the original <see cref="Result" />.</returns>
    public static Task<Result> AsTask(this Result result)
    {
        return Task.FromResult(result);
    }
}