namespace Coderynx.Functional.Results.Errors;

/// <summary>
///     Represents an exception that wraps an <see cref="Error" /> object.
/// </summary>
/// <remarks>
///     This exception is typically used for propagating domain or application errors
///     through the functional result pattern, allowing errors to be thrown as exceptions
///     when necessary.
/// </remarks>
public sealed class ErrorException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ErrorException" /> class with the specified error.
    /// </summary>
    /// <param name="error">
    ///     The error to encapsulate in this exception. The error's message
    ///     is used as the exception message.
    /// </param>
    public ErrorException(Error error) : base(error.Message)
    {
        Error = error;
    }

    /// <summary>
    ///     Gets the original error that caused this exception.
    /// </summary>
    public Error Error { get; }
}