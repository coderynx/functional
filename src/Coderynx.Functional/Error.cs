using Coderynx.Functional.Result;

namespace Coderynx.Functional;

/// <summary>
/// Represents an error with a specific result error, code, and message.
/// </summary>
/// <param name="ResultError">The associated <see cref="ResultError"/> instance that describes the type of error.</param>
/// <param name="Code">A string representing the unique code for the error.</param>
/// <param name="Message">A descriptive message providing details about the error.</param>
public record Error(ResultError ResultError, string Code, string Message)
{
    /// <summary>
    /// A predefined instance of <see cref="Error"/> representing the absence of an error.
    /// </summary>
    public static readonly Error None = new(ResultError.None, "Error.None", "No error");
}