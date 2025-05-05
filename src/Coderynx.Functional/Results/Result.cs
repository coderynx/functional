using Coderynx.Functional.Results.Errors;
using Coderynx.Functional.Results.Successes;

namespace Coderynx.Functional.Results;

/// <summary>
/// Represents the result of an operation that can either succeed or fail.
/// Provides a functional approach to error handling without using exceptions for control flow.
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="success">The success object, or null if the operation failed.</param>
    /// <param name="error">The error object, or null if the operation succeeded.</param>
    /// <exception cref="InvalidOperationException">Thrown when both success and error are null, or when both are provided.</exception>
    internal Result(Success? success = null, Error? error = null)
    {
        if (success is null && error is null)
        {
            throw new InvalidOperationException("Either success or error must be provided.");
        }

        if (success is not null && error is not null)
        {
            throw new InvalidOperationException("Only one of success or error can be provided.");
        }

        Error = error ?? Error.None;
        Success = success ?? Success.None;
    }

    /// <summary>
    /// Gets the error associated with this result, or <see cref="Error.None"/> if this is a success result.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Gets the success object associated with this result, or <see cref="Success.None"/> if this is an error result.
    /// </summary>
    public Success Success { get; }

    /// <summary>
    /// Gets a value indicating whether this result represents a successful operation.
    /// </summary>
    public bool IsSuccess => Success != Success.None && Error == Error.None;

    /// <summary>
    /// Gets a value indicating whether this result represents a failed operation.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Creates a success result with the specified success object.
    /// </summary>
    /// <param name="success">The success object to include in the result.</param>
    /// <returns>A new success result.</returns>
    private static Result CreateSuccess(Success success)
    {
        return new Result(success);
    }

    /// <summary>
    /// Creates a success result with the specified success object containing a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the success object.</typeparam>
    /// <param name="success">The success object containing a value.</param>
    /// <returns>A new success result with a value.</returns>
    private static Result<TValue> CreateSuccess<TValue>(Success<TValue> success)
    {
        return new Result<TValue>(success);
    }

    /// <summary>
    /// Creates a success result representing a resource creation operation.
    /// </summary>
    /// <returns>A success result with <see cref="SuccessKind.Created"/>.</returns>
    public static Result Created()
    {
        return new Result(new Success(SuccessKind.Created));
    }

    /// <summary>
    /// Creates a success result with a value representing a resource creation operation.
    /// </summary>
    /// <typeparam name="TValue">The type of the created value.</typeparam>
    /// <param name="value">The created value.</param>
    /// <returns>A success result with the created value.</returns>
    public static Result<TValue> Created<TValue>(TValue value)
    {
        return CreateSuccess(new Success<TValue>(SuccessKind.Created, value));
    }

    /// <summary>
    /// Creates a success result representing a resource update operation.
    /// </summary>
    /// <returns>A success result with <see cref="SuccessKind.Updated"/>.</returns>
    public static Result Updated()
    {
        return new Result(new Success(SuccessKind.Updated));
    }

    /// <summary>
    /// Creates a success result with a value representing a resource update operation.
    /// </summary>
    /// <typeparam name="TValue">The type of the updated value.</typeparam>
    /// <param name="value">The updated value.</param>
    /// <returns>A success result with the updated value.</returns>
    public static Result<TValue> Updated<TValue>(TValue value)
    {
        return CreateSuccess(new Success<TValue>(SuccessKind.Updated, value));
    }

    /// <summary>
    /// Creates a success result representing a resource retrieval operation.
    /// </summary>
    /// <returns>A success result with <see cref="SuccessKind.Found"/>.</returns>
    public static Result Found()
    {
        return CreateSuccess(new Success(SuccessKind.Found));
    }

    /// <summary>
    /// Creates a success result with a value representing a resource retrieval operation.
    /// </summary>
    /// <typeparam name="TValue">The type of the found value.</typeparam>
    /// <param name="value">The found value.</param>
    /// <returns>A success result with the found value.</returns>
    public static Result<TValue> Found<TValue>(TValue value)
    {
        return CreateSuccess(new Success<TValue>(SuccessKind.Found, value));
    }

    /// <summary>
    /// Creates a success result representing an acceptance operation.
    /// </summary>
    /// <returns>A success result with <see cref="SuccessKind.Accepted"/>.</returns>
    public static Result Accepted()
    {
        return CreateSuccess(new Success(SuccessKind.Accepted));
    }

    /// <summary>
    /// Creates a success result with a value representing an acceptance operation.
    /// </summary>
    /// <typeparam name="TValue">The type of the accepted value.</typeparam>
    /// <param name="value">The accepted value.</param>
    /// <returns>A success result with the accepted value.</returns>
    public static Result<TValue> Accepted<TValue>(TValue value)
    {
        return CreateSuccess(new Success<TValue>(SuccessKind.Accepted, value));
    }

    /// <summary>
    /// Creates a success result representing a deletion operation.
    /// </summary>
    /// <returns>A success result with <see cref="SuccessKind.Deleted"/>.</returns>
    public static Result Deleted()
    {
        return CreateSuccess(new Success(SuccessKind.Deleted));
    }

    /// <summary>
    /// Creates a success result with a value representing a deletion operation.
    /// </summary>
    /// <typeparam name="TValue">The type of the deleted value.</typeparam>
    /// <param name="value">The deleted value.</param>
    /// <returns>A success result with the deleted value.</returns>
    public static Result<TValue> Deleted<TValue>(TValue value)
    {
        return CreateSuccess(new Success<TValue>(SuccessKind.Deleted, value));
    }

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    /// <param name="error">The error to include in the result.</param>
    /// <returns>A new failure result.</returns>
    protected static Result CreateFailure(Error error)
    {
        return new Result(null, error);
    }

    /// <summary>
    /// Creates a failure result with the specified error for a value result.
    /// </summary>
    /// <typeparam name="TValue">The type that would have been returned on success.</typeparam>
    /// <param name="error">The error to include in the result.</param>
    /// <returns>A new failure result.</returns>
    protected static Result<TValue> CreateFailure<TValue>(Error error)
    {
        return new Result<TValue>(null, error);
    }

    /// <summary>
    /// Transforms the result using the appropriate function based on success/failure state.
    /// </summary>
    /// <typeparam name="TOutput">The output type after transformation.</typeparam>
    /// <param name="success">Function to call if this is a success result.</param>
    /// <param name="fail">Function to call if this is a failure result.</param>
    /// <returns>The output of either the success or failure function.</returns>
    public TOutput Match<TOutput>(Func<TOutput> success, Func<Error, TOutput> fail)
    {
        return IsFailure
            ? fail(Error)
            : success();
    }

    /// <summary>
    /// Chains this result to another result-producing function if this is a success.
    /// </summary>
    /// <param name="bind">The function to call if this result is successful.</param>
    /// <returns>The result of the binding function if successful, otherwise this failure result.</returns>
    public Result Bind(Func<Result> bind)
    {
        return IsSuccess ? bind() : this;
    }

    /// <summary>
    /// Chains this result to an asynchronous result-producing function if this is a success.
    /// </summary>
    /// <param name="bind">The asynchronous function to call if this result is successful.</param>
    /// <returns>The result of the binding function if successful, otherwise this failure result.</returns>
    public async Task<Result> BindAsync(Func<Task<Result>> bind)
    {
        return IsSuccess ? await bind() : this;
    }

    /// <summary>
    /// Executes an action within a try-catch block and returns a Result.
    /// </summary>
    /// <param name="onTry">The action to execute within the try block.</param>
    /// <param name="onSuccess">Optional function to create a custom Success object when the action succeeds. 
    /// If not provided, a default Success object is created.</param>
    /// <param name="onCatch">Optional function to convert exceptions into Error objects.
    /// If not provided, exceptions are converted to an Unexpected error.</param>
    /// <param name="onFinally">Optional action to execute in the finally block.</param>
    /// <returns>A Result object representing the outcome of the operation.</returns>
    public static Result TryCatch(
        Action onTry,
        Func<Success>? onSuccess = null,
        Func<Exception, Error>? onCatch = null,
        Action? onFinally = null)
    {
        try
        {
            onTry();

            var success = onSuccess?.Invoke() ?? Success.Custom();
            return CreateSuccess(success);
        }
        catch (ErrorException exception)
        {
            return exception.Error;
        }
        catch (Exception exception)
        {
            return onCatch?.Invoke(exception) ?? Error.Unexpected(exception);
        }
        finally
        {
            onFinally?.Invoke();
        }
    }

    /// <summary>
    /// Executes a function within a try-catch block and returns a Result with the function's return value.
    /// </summary>
    /// <typeparam name="T">The type of value returned by the function.</typeparam>
    /// <param name="onTry">The function to execute within the try block.</param>
    /// <param name="onSuccess">Optional function to create a custom Success object with the value when the function succeeds.
    /// If not provided, a default Success object with the value is created.</param>
    /// <param name="onCatch">Optional function to convert exceptions into Error objects.
    /// If not provided, exceptions are converted to an Unexpected error.</param>
    /// <param name="onFinally">Optional action to execute in the finally block.</param>
    /// <returns>A Result object containing the value or an error representing the outcome of the operation.</returns>
    public static Result<T> TryCatch<T>(
        Func<T> onTry,
        Func<T, Success<T>>? onSuccess = null,
        Func<Exception, Error>? onCatch = null,
        Action? onFinally = null)
    {
        try
        {
            var value = onTry();

            // TODO: Evaluate the possibility to check if value is null.
            var result = onSuccess?.Invoke(value) ?? Success.Custom(value);
            return CreateSuccess(result);
        }
        catch (ErrorException exception)
        {
            return new Error(
                kind: exception.Error.Kind,
                code: exception.Error.Code,
                message: exception.Error.Message);
        }
        catch (Exception exception)
        {
            return onCatch?.Invoke(exception) ?? Error.Unexpected(exception);
        }
        finally
        {
            onFinally?.Invoke();
        }
    }

    public static implicit operator Result(Success success)
    {
        return CreateSuccess(success);
    }

    public static implicit operator Result(Error error)
    {
        return CreateFailure(error);
    }
}

/// <summary>
/// Represents the result of an operation that can either succeed or fail, with a value.
/// Provides a functional approach to error handling without using exceptions for control flow.
///</summary>
/// <typeparam name="TValue">The type of the value contained in the success object.</typeparam>
public sealed class Result<TValue> : Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> class.
    /// </summary>
    /// <param name="value">The success object containing a value, or null if the operation failed.</param>
    /// <param name="error">The error object, or null if the operation succeeded.</param>
    /// <exception cref="InvalidOperationException">Thrown when both value and error are null, or when both are provided.</exception>
    internal Result(Success<TValue>? value = null, Error? error = null) : base(value, error)
    {
        if (value is null && error is null)
        {
            throw new InvalidOperationException("Either value or error must be provided.");
        }

        if (value is not null && error is not null)
        {
            throw new InvalidOperationException("Only one of value or error can be provided.");
        }
    }

    /// <summary>
    /// Gets the success object associated with this result, cast to the appropriate generic type.
    /// </summary>
    public new Success<TValue> Success => (Success<TValue>)base.Success;

    /// <summary>
    /// Gets the value contained in the success object.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to access the value of a failure result.</exception>
    public TValue Value => IsSuccess
        ? Success.Value
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    /// <summary>
    /// Transforms the result using the appropriate function based on success/failure state.
    /// </summary>
    /// <typeparam name="TOutput">The output type after transformation.</typeparam>
    /// <param name="success">Function to call with the value if this is a success result.</param>
    /// <param name="fail">Function to call with the error if this is a failure result.</param>
    /// <returns>The output of either the success or failure function.</returns>
    public TOutput Match<TOutput>(Func<TValue, TOutput> success, Func<Error, TOutput> fail)
    {
        return IsFailure
            ? fail(Error)
            : success(Value);
    }

    /// <summary>
    /// Chains this result to another result-producing function if this is a success.
    /// </summary>
    /// <typeparam name="TNext">The type of the value in the next result.</typeparam>
    /// <param name="bind">The function to call with the value if this result is successful.</param>
    /// <returns>The result of the binding function if successful, otherwise a failure result with the same error.</returns>
    public Result<TNext> Bind<TNext>(Func<TValue, Result<TNext>> bind)
    {
        return IsSuccess ? bind(Value) : CreateFailure<TNext>(Error);
    }

    /// <summary>
    /// Chains this result to an asynchronous result-producing function if this is a success.
    /// </summary>
    /// <typeparam name="TNext">The type of the value in the next result.</typeparam>
    /// <param name="bind">The asynchronous function to call with the value if this result is successful.</param>
    /// <returns>The result of the binding function if successful, otherwise a failure result with the same error.</returns>
    public async Task<Result<TNext>> BindAsync<TNext>(Func<TValue, Task<Result<TNext>>> bind)
    {
        return IsSuccess ? await bind(Value) : CreateFailure<TNext>(Error);
    }

    /// <summary>
    /// Implicitly converts an error to a failure result.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <returns>A failure result containing the specified error.</returns>
    public static implicit operator Result<TValue>(Error error)
    {
        return CreateFailure<TValue>(error);
    }
}