using Coderynx.Functional.Results.Errors;
using Coderynx.Functional.Results.Successes;

namespace Coderynx.Functional.Results;

/// <summary>
///     Represents the result of an operation that can either succeed or fail, with a value.
///     Provides a functional approach to error handling without using exceptions for control flow.
/// </summary>
/// <typeparam name="TValue">The type of the value contained in the success object.</typeparam>
public class Result<TValue> : Result
{
    public Result(Success<TValue> success) : base(success)
    {
    }

    public Result(Error error) : base(error)
    {
    }

    /// <summary>
    ///     Gets the success object associated with this result, cast to the appropriate generic type.
    /// </summary>
    public new Success<TValue> Success => (Success<TValue>)base.Success;

    /// <summary>
    ///     Gets the value contained in the success object.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to access the value of a failure result.</exception>
    public TValue Value => IsSuccess
        ? Success.Value
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    /// <summary>
    ///     Defines an implicit conversion from a <see cref="Success{TValue}" /> object to a <see cref="Result{TValue}" />.
    /// </summary>
    /// <param name="success">The success object containing the value to be encapsulated in the result.</param>
    /// <returns>A new <see cref="Result{TValue}" /> instance that encapsulates the success object.</returns>
    public static implicit operator Result<TValue>(Success<TValue> success)
    {
        return new Result<TValue>(success);
    }

    /// <summary>
    ///     Implicitly converts an error to a failure result.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <returns>A failure result containing the specified error.</returns>
    public static implicit operator Result<TValue>(Error error)
    {
        return new Result<TValue>(error);
    }
}