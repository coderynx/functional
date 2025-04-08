namespace Coderynx.Functional.Result;

/// <summary>
///     Represents the possible error types for a result in the functional programming paradigm.
/// </summary>
public enum ResultError
{
    /// <summary>
    ///     Indicates that no error occurred.
    /// </summary>
    None,

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
    ///     Represents a custom error type defined by the user.
    /// </summary>
    Custom,

    /// <summary>
    ///     Indicates that an invalid operation was attempted.
    /// </summary>
    InvalidOperation
}