namespace Coderynx.Functional.Results.Errors;

/// <summary>
///     Represents an error that occurred during an operation.
/// </summary>
public record Error
{
    /// <summary>
    ///     Represents the absence of an error.
    /// </summary>
    public static readonly Error None = new(ErrorKind.None, "Error.None", "No error");

    /// <summary>
    ///     Initializes a new instance of the <see cref="Error" /> class.
    /// </summary>
    /// <param name="kind">The kind of error.</param>
    /// <param name="code">A code that identifies the error.</param>
    /// <param name="message">A human-readable description of the error.</param>
    internal Error(ErrorKind kind, string code, string message)
    {
        Kind = kind;
        Code = code;
        Message = message;
    }

    /// <summary>
    ///     Gets the kind of the error.
    /// </summary>
    public ErrorKind Kind { get; }

    /// <summary>
    ///     Gets a code that identifies the error.
    /// </summary>
    public string Code { get; }

    /// <summary>
    ///     Gets a human-readable description of the error.
    /// </summary>
    public string Message { get; }

    /// <summary>
    ///     Creates an error from an unexpected exception.
    /// </summary>
    /// <typeparam name="TException">The type of the exception.</typeparam>
    /// <param name="exception">The exception that occurred.</param>
    /// <returns>An error representing the unexpected exception.</returns>
    internal static Error Unexpected<TException>(TException exception) where TException : Exception
    {
        return new Error(
            kind: ErrorKind.Unexpected,
            code: "Error.Unexpected",
            message: $"An exception of type {typeof(TException).Name} occured: {exception.Message}");
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> class with an unexpected error kind.
    /// </summary>
    /// <param name="code">A code that identifies the unexpected error.</param>
    /// <param name="message">A human-readable description of the unexpected error.</param>
    /// <returns>An instance of the <see cref="Error" /> class representing the unexpected error.</returns>
    public static Error Unexpected(string code, string message)
    {
        return new Error(ErrorKind.Unexpected, code, message);
    }

    /// <summary>
    ///     Creates a not found error.
    /// </summary>
    /// <param name="code">A code that identifies the error.</param>
    /// <param name="message">A human-readable description of the error.</param>
    /// <returns>A not found error.</returns>
    public static Error NotFound(string code, string message)
    {
        return new Error(ErrorKind.NotFound, code, message);
    }

    /// <summary>
    ///     Creates a conflict error.
    /// </summary>
    /// <param name="code">A code that identifies the error.</param>
    /// <param name="message">A human-readable description of the error.</param>
    /// <returns>A conflict error.</returns>
    public static Error Conflict(string code, string message)
    {
        return new Error(ErrorKind.Conflict, code, message);
    }

    /// <summary>
    ///     Creates an invalid input error.
    /// </summary>
    /// <param name="code">A code that identifies the error.</param>
    /// <param name="message">A human-readable description of the error.</param>
    /// <returns>An invalid input error.</returns>
    public static Error InvalidInput(string code, string message)
    {
        return new Error(ErrorKind.InvalidInput, code, message);
    }

    /// <summary>
    ///     Creates an invalid operation error.
    /// </summary>
    /// <param name="code">A code that identifies the error.</param>
    /// <param name="message">A human-readable description of the error.</param>
    /// <returns>An invalid operation error.</returns>
    public static Error InvalidOperation(string code, string message)
    {
        return new Error(ErrorKind.InvalidOperation, code, message);
    }

    /// <summary>
    ///     Creates a custom error.
    /// </summary>
    /// <param name="code">A code that identifies the error.</param>
    /// <param name="message">A human-readable description of the error.</param>
    /// <returns>A custom error.</returns>
    public static Error Custom(string code, string message)
    {
        return new Error(ErrorKind.Custom, code, message);
    }

    /// <summary>
    ///     Implicitly converts an error to an error exception.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    public static implicit operator ErrorException(Error error)
    {
        return new ErrorException(error);
    }
}