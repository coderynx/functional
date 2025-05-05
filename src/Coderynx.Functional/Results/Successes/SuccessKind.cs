namespace Coderynx.Functional.Results.Successes;

/// <summary>
///     Represents the success states of a result operation.
/// </summary>
public enum SuccessKind
{
    /// <summary>
    ///     Indicates that no success state is defined.
    /// </summary>
    None,

    /// <summary>
    ///     Indicates that the requested resource was found.
    /// </summary>
    Found,

    /// <summary>
    ///     Indicates that a new resource was successfully created.
    /// </summary>
    Created,

    /// <summary>
    ///     Indicates that an existing resource was successfully updated.
    /// </summary>
    Updated,

    /// <summary>
    ///     Indicates that a resource was successfully deleted.
    /// </summary>
    Deleted,

    /// <summary>
    ///     Indicates that the operation was accepted for processing.
    /// </summary>
    Accepted,

    /// <summary>
    ///     Indicates that the operation was successful.
    /// </summary>
    Custom
}