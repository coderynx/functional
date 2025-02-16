using Coderynx.Functional.Result;

namespace Coderynx.Functional;

public record Error(Result.ResultError ResultError, string Code, string Message)
{
    public static readonly Error None = new(ResultError.None, "Error.None", "No error");
}