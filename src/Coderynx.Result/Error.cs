namespace Coderynx.Result;

public record Error(ResultError ResultError, string Code, string Message)
{
    public static readonly Error None = new(ResultError.None, "Error.None", "No error");
}