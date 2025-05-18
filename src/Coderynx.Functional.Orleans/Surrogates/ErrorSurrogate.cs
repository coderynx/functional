using Coderynx.Functional.Results.Errors;
using Orleans;

namespace Coderynx.Functional.Orleans.Surrogates;

[GenerateSerializer]
internal readonly struct ErrorSurrogate(ErrorKind kind, string code, string message)
{
    [Id(0)] public ErrorKind Kind { get; } = kind;
    [Id(1)] public string Code { get; } = code;
    [Id(2)] public string Message { get; } = message;
}

[RegisterConverter]
internal sealed class ErrorSurrogateConverter : IConverter<Error, ErrorSurrogate>
{
    public Error ConvertFromSurrogate(in ErrorSurrogate surrogate)
    {
        return surrogate.Kind switch
        {
            ErrorKind.None => Error.None,
            ErrorKind.Unexpected => Error.Unexpected(surrogate.Code, surrogate.Message),
            ErrorKind.NotFound => Error.NotFound(surrogate.Code, surrogate.Message),
            ErrorKind.Conflict => Error.Conflict(surrogate.Code, surrogate.Message),
            ErrorKind.InvalidInput => Error.InvalidInput(surrogate.Code, surrogate.Message),
            ErrorKind.InvalidOperation => Error.InvalidOperation(surrogate.Code, surrogate.Message),
            ErrorKind.Custom => Error.Custom(surrogate.Code, surrogate.Message),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public ErrorSurrogate ConvertToSurrogate(in Error value)
    {
        return new ErrorSurrogate(value.Kind, value.Code, value.Message);
    }
}