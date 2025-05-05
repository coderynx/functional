namespace Coderynx.Functional.Results.Errors;

/// <summary>
///     Represents the possible error types for a result in the functional programming paradigm.
/// </summary>
public enum ErrorKind
{
    /// <summary>
    ///     Indicates that no error occurred.
    /// </summary>
    None,

    /// <summary>
    ///     Indicates that the operation was successful.
    /// </summary>
    Unexpected,

    /// <summary>
    ///     Indicates that the requested resource was not found.
    /// </summary>
    NotFound,

    /// <summary>
    ///     Indicates a conflict occurred, such as a duplicate resource.
    /// </summary>
    Conflict,

    /// <summary>
    ///     Indicates that the input provided was invalid.
    /// </summary>
    InvalidInput,

    /// <summary>
    ///     Indicates that an invalid operation was attempted.
    /// </summary>
    InvalidOperation,

    /// <summary>
    ///     Represents a custom error type defined by the user.
    /// </summary>
    Custom
}