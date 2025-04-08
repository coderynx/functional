namespace Coderynx.Functional.Results;

/// <summary>
///     Represents the result of an operation, encapsulating success or failure states.
/// </summary>
public class Result
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Result" /> class.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the operation was successful.</param>
    /// <param name="error">The error associated with the result, if any.</param>
    /// <param name="successType">The type of success, if applicable.</param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the combination of <paramref name="isSuccess" /> and <paramref name="error" /> is invalid.
    /// </exception>
    protected Result(bool isSuccess, Error error, ResultSuccess? successType = null)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
            case false when error == Error.None:
                throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
        SuccessType = successType;
    }

    /// <summary>
    ///     Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    ///     Gets a value indicating whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    ///     Gets the error associated with the result, if any.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    ///     Gets the type of success, if applicable.
    /// </summary>
    public ResultSuccess? SuccessType { get; }

    /// <summary>
    ///     Creates a successful result with a specified success type.
    /// </summary>
    /// <param name="resultSuccess">The type of success.</param>
    /// <returns>A successful <see cref="Result" />.</returns>
    public static Result Success(ResultSuccess resultSuccess)
    {
        return new Result(true, Error.None, resultSuccess);
    }

    /// <summary>
    ///     Creates a successful result with a value and a specified success type.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value associated with the result.</param>
    /// <param name="resultSuccess">The type of success.</param>
    /// <returns>A successful <see cref="Result{TValue}" />.</returns>
    public static Result<TValue> Success<TValue>(TValue value, ResultSuccess resultSuccess)
    {
        return new Result<TValue>(value, true, Error.None, resultSuccess);
    }

    /// <summary>
    ///     Creates a result indicating an update operation was successful.
    /// </summary>
    /// <returns>A successful <see cref="Result" /> with the "Updated" success type.</returns>
    public static Result Updated()
    {
        return Success(ResultSuccess.Updated);
    }

    /// <summary>
    ///     Creates a result indicating a creation operation was successful.
    /// </summary>
    /// <returns>A successful <see cref="Result" /> with the "Created" success type.</returns>
    public static Result Created()
    {
        return Success(ResultSuccess.Created);
    }

    /// <summary>
    ///     Creates a result indicating a creation operation was successful, with a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value associated with the result.</param>
    /// <returns>A successful <see cref="Result{TValue}" /> with the "Created" success type.</returns>
    public static Result<TValue> Created<TValue>(TValue value)
    {
        return Success(value, ResultSuccess.Created);
    }

    /// <summary>
    ///     Creates a result indicating a value was found.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value associated with the result.</param>
    /// <returns>A successful <see cref="Result{TValue}" /> with the "Found" success type.</returns>
    public static Result<TValue> Found<TValue>(TValue value)
    {
        return Success(value, ResultSuccess.Found);
    }

    /// <summary>
    ///     Creates a result indicating an operation was accepted.
    /// </summary>
    /// <returns>A successful <see cref="Result" /> with the "Accepted" success type.</returns>
    public static Result Accepted()
    {
        return Success(ResultSuccess.Accepted);
    }

    /// <summary>
    ///     Creates a result indicating a deletion operation was successful.
    /// </summary>
    /// <returns>A successful <see cref="Result" /> with the "Deleted" success type.</returns>
    public static Result Deleted()
    {
        return Success(ResultSuccess.Deleted);
    }

    /// <summary>
    ///     Creates a failed result with a specified error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A failed <see cref="Result" />.</returns>
    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }

    /// <summary>
    ///     Creates a failed result with a specified error and value type.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A failed <see cref="Result{TValue}" />.</returns>
    public static Result<TValue> Failure<TValue>(Error error)
    {
        return new Result<TValue>(default, false, error);
    }

    /// <summary>
    ///     Matches the result to a success or failure function and returns the output.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="success">The function to execute if the result is successful.</param>
    /// <param name="fail">The function to execute if the result is a failure.</param>
    /// <returns>The output of the matched function.</returns>
    public TOutput Match<TOutput>(Func<TOutput> success, Func<Error, TOutput> fail)
    {
        return IsFailure
            ? fail(Error)
            : success();
    }

    /// <summary>
    /// Chains the current result with another operation.
    /// </summary>
    /// <param name="bind">The function to execute if the current result is successful.</param>
    /// <returns>The resulting <see cref="Result"/> or propagates the failure.</returns>
    public Result Bind(Func<Result> bind)
    {
        return IsSuccess ? bind() : this;
    }
    
    /// <summary>
    ///     Implicitly converts an <see cref="Error" /> to a failed <see cref="Result" />.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    public static implicit operator Result(Error error)
    {
        return Failure(error);
    }
}

/// <summary>
///     Represents the result of an operation with an associated value.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result{TValue}" /> class.
    /// </summary>
    /// <param name="value">The value associated with the result.</param>
    /// <param name="isSuccess">Indicates whether the operation was successful.</param>
    /// <param name="error">The error associated with the result, if any.</param>
    /// <param name="successType">The type of success, if applicable.</param>
    protected internal Result(TValue? value, bool isSuccess, Error error, ResultSuccess? successType = null)
        : base(isSuccess, error, successType)
    {
        _value = value;
    }

    /// <summary>
    ///     Gets a value indicating whether the result has a value.
    /// </summary>
    public bool HasValue => IsSuccess && _value is not null;

    /// <summary>
    ///     Gets the value associated with the result.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when accessing the value of a failed result.</exception>
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");


    /// <summary>
    ///     Matches the result to a success or failure function and returns the output.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="success">The function to execute if the result is successful.</param>
    /// <param name="fail">The function to execute if the result is a failure.</param>
    /// <returns>The output of the matched function.</returns>
    public TOutput Match<TOutput>(Func<TValue, TOutput> success, Func<Error, TOutput> fail)
    {
        return IsFailure
            ? fail(Error)
            : success(Value);
    }
    
    /// <summary>
    /// Chains the current result with another operation that depends on the value.
    /// </summary>
    /// <typeparam name="TNext">The type of the next result's value.</typeparam>
    /// <param name="bind">The function to execute if the current result is successful.</param>
    /// <returns>The resulting <see cref="Result{TNext}"/> or propagates the failure.</returns>
    public Result<TNext> Bind<TNext>(Func<TValue, Result<TNext>> bind)
    {
        return IsSuccess ? bind(Value) : Failure<TNext>(Error);
    }

    /// <summary>
    ///     Implicitly converts an <see cref="Error" /> to a failed <see cref="Result{TValue}" />.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    public static implicit operator Result<TValue>(Error error)
    {
        return Failure<TValue>(error);
    }
}