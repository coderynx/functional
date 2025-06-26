using Coderynx.Functional.Results;
using Coderynx.Functional.Results.Errors;
using Coderynx.Functional.Results.Successes;

namespace Coderynx.Functional.Orleans.Surrogates;

[GenerateSerializer]
internal readonly struct TypedResultSurrogate<TValue>(
    bool isSuccess,
    Error error,
    Success<TValue> success) where TValue : class
{
    [Id(0)] public bool IsSuccess { get; } = isSuccess;
    [Id(1)] public Error Error { get; } = error;
    [Id(2)] public Success<TValue> Success { get; } = success;
}

[RegisterConverter]
internal sealed class TypedResultSurrogateConverter<TValue> : IConverter<Result<TValue>, TypedResultSurrogate<TValue>>
    where TValue : class
{
    public Result<TValue> ConvertFromSurrogate(in TypedResultSurrogate<TValue> surrogate)
    {
        return surrogate.IsSuccess
            ? surrogate.Success
            : surrogate.Error;
    }

    public TypedResultSurrogate<TValue> ConvertToSurrogate(in Result<TValue> value)
    {
        return new TypedResultSurrogate<TValue>(value.IsSuccess, value.Error, value.Success);
    }
}