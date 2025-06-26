using Coderynx.Functional.Results.Errors;
using Coderynx.Functional.Results.Successes;

namespace Coderynx.Functional.Results;

/// <summary>
///     Provides extension methods for handling and manipulating <see cref="Result{TValue}" /> objects in a functional
///     manner.
/// </summary>
public static class ResultTValueExtensions
{
    /// <summary>
    ///     Matches the result state to the corresponding function and returns the mapped output.
    ///     Executes the <paramref name="success" /> function if the result is successful, otherwise executes the
    ///     <paramref name="error" /> function.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output returned by the matching function.</typeparam>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result to be matched.</param>
    /// <param name="success">
    ///     A function to be executed if the result is successful, which takes the success value as a
    ///     parameter.
    /// </param>
    /// <param name="error">A function to be executed if the result is a failure, which takes the failure error as a parameter.</param>
    /// <returns>
    ///     The output of the executed function, either from the <paramref name="success" /> or <paramref name="error" />
    ///     function based on the result state.
    /// </returns>
    public static TOutput Match<TOutput, TValue>(
        this Result<TValue> result,
        Func<TValue, TOutput> success,
        Func<Error, TOutput> error)
    {
        return result.IsFailure
            ? error(result.Error)
            : success(result.Value);
    }

    /// <summary>
    ///     Matches the asynchronous result state to the corresponding asynchronous function
    ///     and returns the mapped output as a task.
    ///     Executes the <paramref name="success" /> function if the result is successful,
    ///     otherwise executes the <paramref name="error" /> function.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output returned by the matching function.</typeparam>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result to be matched.</param>
    /// <param name="success">
    ///     An asynchronous function to be executed if the result is successful, which takes the success
    ///     value as a parameter.
    /// </param>
    /// <param name="error">
    ///     An asynchronous function to be executed if the result is a failure, which takes the failure error
    ///     as a parameter.
    /// </param>
    /// <returns>
    ///     A task representing the output of the executed asynchronous function, either from the
    ///     <paramref name="success" /> or <paramref name="error" /> function based on the result state.
    /// </returns>
    public static async Task<TOutput> MatchAsync<TOutput, TValue>(
        this Result<TValue> result,
        Func<TValue, Task<TOutput>> success,
        Func<Error, Task<TOutput>> error)
    {
        return result.IsFailure
            ? await error(result.Error)
            : await success(result.Value);
    }

    /// <summary>
    ///     Matches the result state of a task containing a result to the corresponding asynchronous function and returns the
    ///     mapped asynchronous output.
    ///     Executes the <paramref name="success" /> function if the result is successful, otherwise executes the
    ///     <paramref name="error" /> function.
    /// </summary>
    /// <typeparam name="TOutput">The type of the asynchronous output returned by the matching function.</typeparam>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="resultTask">A task containing the result to be matched.</param>
    /// <param name="success">
    ///     An asynchronous function to be executed if the result is successful, which takes the success
    ///     value as a parameter.
    /// </param>
    /// <param name="error">
    ///     An asynchronous function to be executed if the result is a failure, which takes the failure error
    ///     as a parameter.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the output of the executed function,
    ///     either from the <paramref name="success" /> or <paramref name="error" /> function based on the result state.
    /// </returns>
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

    /// <summary>
    ///     Filters the result by applying the specified <paramref name="predicate" /> function to the result's error if the
    ///     result is not successful.
    ///     If the result is successful, it remains unchanged.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result to be filtered.</param>
    /// <param name="predicate">A function to transform the error of the result if it is a failure.</param>
    /// <returns>
    ///     The original result if it is successful, otherwise a new failure result with the transformed error returned by the
    ///     <paramref name="predicate" /> function.
    /// </returns>
    public static Result<TValue> Filter<TValue>(
        this Result<TValue> result,
        Func<Error, Error> predicate)
    {
        return result.IsSuccess
            ? result
            : new Result<TValue>(predicate(result.Error));
    }

    /// <summary>
    ///     Applies an asynchronous filtering function to a failed result, modifying the error if the result is not successful.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result to be filtered. If successful, it is returned untouched.</param>
    /// <param name="predicate">An asynchronous function that takes the existing error and returns a transformed error.</param>
    /// <returns>
    ///     An asynchronous result containing the original success value if the result is successful.
    ///     If the result is a failure, it contains the result with the error transformed by the <paramref name="predicate" />
    ///     function.
    /// </returns>
    public static async Task<Result<TValue>> FilterAsync<TValue>(
        this Result<TValue> result,
        Func<Error, Task<Error>> predicate)
    {
        return result.IsSuccess
            ? result
            : new Result<TValue>(await predicate(result.Error));
    }

    /// <summary>
    ///     Applies the specified asynchronous predicate to the error of a result task.
    ///     If the result indicates failure, the predicate is evaluated and the resulting error is returned.
    ///     If the result is successful, it is returned as-is.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="resultTask">A task that resolves to the result to be filtered.</param>
    /// <param name="predicate">An asynchronous function that transforms the error if the result indicates failure.</param>
    /// <returns>
    ///     A task that resolves to a filtered result. If the result was successful, no changes are made.
    ///     If the result was a failure, the updated error from the predicate is applied.
    /// </returns>
    public static async Task<Result<TValue>> FilterAsync<TValue>(
        this Task<Result<TValue>> resultTask,
        Func<Error, Task<Error>> predicate)
    {
        var result = await resultTask;
        return result.IsSuccess
            ? result
            : new Result<TValue>(await predicate(result.Error));
    }

    /// <summary>
    ///     Flattens a nested <see cref="Result{TValue}" /> instance by unwrapping the inner result if the outer result is
    ///     successful.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the inner result.</typeparam>
    /// <param name="result">The nested result to be flattened.</param>
    /// <returns>
    ///     A single <see cref="Result{TValue}" /> instance. If the outer result is successful, the inner result is returned.
    ///     If the outer result is a failure, a failure result with the same error is returned.
    /// </returns>
    public static Result<TValue> Flatten<TValue>(this Result<Result<TValue>> result)
    {
        return result.IsSuccess ? result.Value : new Result<TValue>(result.Error);
    }

    /// <summary>
    ///     Flattens a nested <see cref="Result{TValue}" /> asynchronously by resolving outer and inner results
    ///     into a single result. If the outer result is successful, the inner result is returned. Otherwise, the error from
    ///     the outer result is propagated.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the inner result.</typeparam>
    /// <param name="resultTask">
    ///     A task representing an asynchronous operation that produces a <see cref="Result{TValue}" />
    ///     containing another result.
    /// </param>
    /// <returns>
    ///     A task that resolves to a flat <see cref="Result{TValue}" />, with either the value from the inner result or
    ///     the error from the outer result.
    /// </returns>
    public static async Task<Result<TValue>> FlattenAsync<TValue>(
        this Task<Result<Result<TValue>>> resultTask)
    {
        var result = await resultTask;
        return result.IsSuccess ? result.Value : new Result<TValue>(result.Error);
    }

    /// <summary>
    ///     Transforms the success value of the result using the provided mapping function, returning a new result with the
    ///     transformed value.
    ///     If the result is a failure, the original failure is returned without transformation.
    /// </summary>
    /// <typeparam name="TNext">The type of the transformed value.</typeparam>
    /// <typeparam name="TValue">The type of the original value contained in the result.</typeparam>
    /// <param name="result">The result to be mapped.</param>
    /// <param name="map">The function used to transform the successful value of the result.</param>
    /// <returns>
    ///     A new result containing the transformed value if the original result was successful, or the original failure
    ///     otherwise.
    /// </returns>
    public static Result<TNext> Map<TNext, TValue>(this Result<TValue> result, Func<TValue, TNext> map)
    {
        return result.IsSuccess
            ? new Result<TNext>(new Success<TNext>(result.Success.Kind, map(result.Value)))
            : new Result<TNext>(result.Error);
    }

    /// <summary>
    ///     Asynchronously maps the value of a successful result to a new result with a different type using the provided
    ///     mapping function.
    ///     If the result is a failure, it passes the failure error to the new result without invoking the mapping function.
    /// </summary>
    /// <typeparam name="TNext">The type of the value in the resulting mapped result.</typeparam>
    /// <typeparam name="TValue">The type of the value in the initial result.</typeparam>
    /// <param name="result">The initial result to be mapped.</param>
    /// <param name="map">An asynchronous function to map the value of a successful result to a new type.</param>
    /// <returns>
    ///     An asynchronous operation that returns a new result containing the mapped value if the initial result is
    ///     successful, or the original error if the result is a failure.
    /// </returns>
    public static async Task<Result<TNext>> MapAsync<TNext, TValue>(
        this Result<TValue> result,
        Func<TValue, Task<TNext>> map)
    {
        return result.IsSuccess
            ? new Result<TNext>(new Success<TNext>(result.Success.Kind, await map(result.Value)))
            : new Result<TNext>(result.Error);
    }

    /// <summary>
    /// Applies an asynchronous mapping function to the value of a successful result contained within a
    /// task-wrapped result, producing a new result with the mapped value.
    /// If the result is a failure, the error is propagated unchanged within the returned result.
    /// </summary>
    /// <typeparam name="TNext">The type of the transformed value in the returned result.</typeparam>
    /// <typeparam name="TValue">The type of the value contained in the original result.</typeparam>
    /// <param name="resultTask">A task that resolves to the result to be mapped.</param>
    /// <param name="map">An asynchronous mapping function to be applied to the value of a successful result.</param>
    /// <returns>
    /// A task that resolves to a result containing the mapped value for a successful result or the original error
    /// for a failure result.
    /// </returns>
    public static async Task<Result<TNext>> MapAsync<TNext, TValue>(
        this Task<Result<TValue>> resultTask,
        Func<TValue, Task<TNext>> map)
    {
        var result = await resultTask;
        return result.IsSuccess
            ? new Result<TNext>(new Success<TNext>(result.Success.Kind, await map(result.Value)))
            : new Result<TNext>(result.Error);
    }

    /// <summary>
    ///     Transforms the value of a successful result using the provided <paramref name="bind" /> function, returning a new
    ///     result.
    ///     If the result is a failure, it returns a failure with the same error.
    /// </summary>
    /// <typeparam name="TNext">
    ///     The type of the value contained in the result returned by the <paramref name="bind" />
    ///     function.
    /// </typeparam>
    /// <typeparam name="TValue">The type of the value contained in the input result.</typeparam>
    /// <param name="result">
    ///     The input result on which the <paramref name="bind" /> function will be applied if it is
    ///     successful.
    /// </param>
    /// <param name="bind">
    ///     A function that maps the value of the input result to a new result of type
    ///     <typeparamref name="TNext" />.
    /// </param>
    /// <returns>
    ///     A new result of type <typeparamref name="TNext" />. If the input result is successful, the returned result is
    ///     determined by the <paramref name="bind" /> function.
    ///     If the input result is a failure, the returned result is a failure with the same error.
    /// </returns>
    public static Result<TNext> Bind<TNext, TValue>(this Result<TValue> result, Func<TValue, Result<TNext>> bind)
    {
        return result.IsSuccess ? bind(result.Value) : new Result<TNext>(result.Error);
    }

    /// <summary>
    /// Asynchronously binds the result's value to the specified function, returning a new asynchronous result.
    /// Executes the <paramref name="bind" /> function if the result is successful or preserves the current error otherwise.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result to be bound.</param>
    /// <param name="bind">
    /// A function to be executed if the result is successful, which takes the success value as a parameter and returns
    /// a <see cref="Task{Result}" />.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous binding operation. The resultant task contains a new result based on the
    /// outcome of the binding function or the original error if the result was not successful.
    /// </returns>
    public static async Task<Result> BindAsync<TValue>(
        this Result<TValue> result,
        Func<TValue, Task<Result>> bind)
    {
        return result.IsSuccess ? await bind(result.Value) : new Result(result.Error);
    }

    /// <summary>
    /// Asynchronously binds the result of the task-based result to a function that returns a new task-based result.
    /// If the result is successful, executes the <paramref name="bind" /> function with the successful value.
    /// Otherwise, returns the error from the original result.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the original result.</typeparam>
    /// <param name="resultTask">The task representing the original result to be bound.</param>
    /// <param name="bind">
    /// A function to be executed if the original result is successful, which takes the value as a parameter
    /// and returns a new task-based result.
    /// </param>
    /// <returns>
    /// A task representing the resulting operation, which will contain the result of executing the
    /// <paramref name="bind" /> function or the original error if the result was unsuccessful.
    /// </returns>
    public static async Task<Result> BindAsync<TValue>(
        this Task<Result<TValue>> resultTask,
        Func<TValue, Task<Result>> bind)
    {
        var result = await resultTask;
        return result.IsSuccess ? await bind(result.Value) : result.Error;
    }

    /// <summary>
    ///     Asynchronously binds the given successful result to a function that produces another asynchronous result
    ///     and returns the new result. If the given result is a failure, it returns a failure with the same error.
    /// </summary>
    /// <typeparam name="TNext">The type of the value contained in the resulting bound <see cref="Result{TNext}" />.</typeparam>
    /// <typeparam name="TValue">The type of the value contained in the input <see cref="Result{TValue}" />.</typeparam>
    /// <param name="result">The input result to be bound.</param>
    /// <param name="bind">A function that takes the successful value of the input result and produces an asynchronous result.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing the new result produced by the bind function
    ///     upon success, or a failure with the same error upon failure.
    /// </returns>
    public static async Task<Result<TNext>> BindAsync<TNext, TValue>(
        this Result<TValue> result,
        Func<TValue, Task<Result<TNext>>> bind)
    {
        return result.IsSuccess ? await bind(result.Value) : new Result<TNext>(result.Error);
    }

    /// <summary>
    ///     Asynchronously applies a binding function to a result, transforming its value if successful.
    ///     If the original result is a failure, the failure is propagated without invoking the binding function.
    /// </summary>
    /// <typeparam name="TNext">The type of the value in the resulting transformed <see cref="Result{TNext}" />.</typeparam>
    /// <typeparam name="TValue">The type of the value in the initial result.</typeparam>
    /// <param name="resultTask">A task representing the result to which the bind function will be applied.</param>
    /// <param name="bind">
    ///     An asynchronous function that takes a value of type <typeparamref name="TValue" /> and returns a
    ///     task of <see cref="Result{TNext}" />.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation, which resolves to a <see cref="Result{TNext}" />.
    ///     If the original result is successful, the task resolves to the result of the bind function.
    ///     Otherwise, it resolves to the propagated failure from the original result.
    /// </returns>
    public static async Task<Result<TNext>> BindAsync<TNext, TValue>(
        this Task<Result<TValue>> resultTask,
        Func<TValue, Task<Result<TNext>>> bind)
    {
        var result = await resultTask;
        return result.IsSuccess ? await bind(result.Value) : new Result<TNext>(result.Error);
    }

    /// <summary>
    ///     Asynchronously applies the specified bind function to the result's value if it is successful.
    ///     If the result is a failure, the same failure is returned without applying the bind function.
    /// </summary>
    /// <typeparam name="TNext">The type of the result produced by the bind function.</typeparam>
    /// <typeparam name="TValue">The type of the value contained in the original result.</typeparam>
    /// <param name="resultTask">A task that represents the original result to operate on.</param>
    /// <param name="bind">A function to apply to the value of a successful result, producing a new result.</param>
    /// <returns>
    ///     A task that represents the result of applying the bind function if the original result is successful, or the
    ///     original failure result if it is not.
    /// </returns>
    public static async Task<Result<TNext>> BindAsync<TNext, TValue>(
        this Task<Result<TValue>> resultTask,
        Func<TValue, Result<TNext>> bind)
    {
        var result = await resultTask;
        return result.IsSuccess ? bind(result.Value) : new Result<TNext>(result.Error);
    }

    /// <summary>
    /// Transforms the value of a successful result asynchronously using the provided mapping function.
    /// If the result is a failure, it returns the failure as-is.
    /// </summary>
    /// <typeparam name="TNext">The type of the transformed value.</typeparam>
    /// <typeparam name="TValue">The type of the value contained in the original result.</typeparam>
    /// <param name="resultTask">A task representing the result to be transformed.</param>
    /// <param name="map">A function to transform the value of a successful result.</param>
    /// <returns>
    /// A task representing the transformed result. If the original result is successful, the task contains the
    /// transformed result. Otherwise, the task contains the same failure result.
    /// </returns>
    public static async Task<Result<TNext>> MapAsync<TNext, TValue>(
        this Task<Result<TValue>> resultTask,
        Func<TValue, TNext> map)
    {
        var result = await resultTask;
        return result.IsSuccess 
            ? new Result<TNext>(new Success<TNext>(result.Success.Kind, map(result.Value)))
            : new Result<TNext>(result.Error);
    }
    
    /// <summary>
    ///     Executes a specified action if the result state is successful and returns the original result.
    ///     This method allows performing side effects without altering the result's state or value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result on which the action will be performed if it is successful.</param>
    /// <param name="action">The action to execute on the contained value when the result is successful.</param>
    /// <returns>The original result, unchanged, regardless of the operation performed.</returns>
    public static Result<TValue> Tap<TValue>(this Result<TValue> result, Action<TValue> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value);
        }

        return result;
    }

    /// <summary>
    ///     Executes an asynchronous action on the value of a successful result without modifying the result.
    ///     If the result is not successful, no action is performed.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result to perform the action on.</param>
    /// <param name="action">
    ///     An asynchronous action to execute if the result is successful, which takes the success value as a
    ///     parameter.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the original result after the action is executed
    ///     (if applicable).
    /// </returns>
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

    /// <summary>
    ///     Applies an asynchronous action to the success value of the task-based result if it is successful.
    ///     The action is executed only if the result's state is successful, passing the success value to the action.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="resultTask">The task representing the result to which the asynchronous action will be applied.</param>
    /// <param name="action">
    ///     An asynchronous action to be executed if the result is successful, taking the success value as a
    ///     parameter.
    /// </param>
    /// <returns>
    ///     A task representing the result after the action has been executed on the success value, or the original result
    ///     if it is not successful.
    /// </returns>
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

    /// <summary>
    ///     Executes the provided asynchronous side effect function if the result is successful.
    ///     Returns the original result regardless of the operation's outcome.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result on which to conditionally execute the side effect.</param>
    /// <param name="action">An asynchronous function to be executed as a side effect if the result is successful.</param>
    /// <returns>The original result, regardless of whether the side effect was executed.</returns>
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

    /// <summary>
    ///     Executes a specified asynchronous action if the wrapped result is successful.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="resultTask">The task representing the asynchronous operation returning a result.</param>
    /// <param name="action">The asynchronous action to be executed if the result is successful.</param>
    /// <returns>A task representing the asynchronous operation, yielding the original result.</returns>
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

    /// <summary>
    ///     Ensures that the given <paramref name="predicate" /> is satisfied by the value in the result.
    ///     If the predicate is not satisfied, the result is transformed into a failure with the provided
    ///     <paramref name="onFailure" /> error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result containing a value to validate against the predicate.</param>
    /// <param name="predicate">A function that defines the condition to ensure for the value.</param>
    /// <param name="onFailure">A function that generates an error to be used if the predicate is not satisfied.</param>
    /// <returns>
    ///     Returns the original result if it is already a failure or if the predicate is satisfied.
    ///     Otherwise, returns a failure result with the error provided by <paramref name="onFailure" />.
    /// </returns>
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

    /// <summary>
    ///     Ensures that the result satisfies a specified asynchronous predicate.
    ///     If the predicate returns false, converts the result to a failure with the provided error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result to be validated.</param>
    /// <param name="predicate">An asynchronous predicate to evaluate the value of the result.</param>
    /// <param name="onFailure">A function that provides an error to use if the predicate evaluation fails.</param>
    /// <returns>
    ///     The original result if the predicate is satisfied, or a failed result with the specified error if the
    ///     predicate is not satisfied.
    /// </returns>
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

    /// <summary>
    ///     Asynchronously ensures that a successful result meets a specified predicate. If the predicate is not met, returns a
    ///     failure with the provided error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="resultTask">The task of the result to be evaluated.</param>
    /// <param name="predicate">
    ///     An asynchronous function to test the value contained in the result. Returns true if the
    ///     condition is met, false otherwise.
    /// </param>
    /// <param name="onFailure">A function returning an error to be used if the predicate is not satisfied.</param>
    /// <returns>
    ///     An asynchronous operation that results in the original successful result if the predicate is satisfied, or a
    ///     failure result with the provided error if the predicate is not met. If the initial result is a failure, it is
    ///     returned unchanged.
    /// </returns>
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