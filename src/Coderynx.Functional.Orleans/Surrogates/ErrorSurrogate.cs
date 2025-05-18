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
        return new Error(surrogate.Kind, surrogate.Code, surrogate.Message);
    }

    public ErrorSurrogate ConvertToSurrogate(in Error value)
    {
        return new ErrorSurrogate(value.Kind, value.Code, value.Message);
    }
}