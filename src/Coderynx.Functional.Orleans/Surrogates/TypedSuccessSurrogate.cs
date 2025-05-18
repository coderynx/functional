using Coderynx.Functional.Results.Successes;
using Orleans;

namespace Coderynx.Functional.Orleans.Surrogates;

[GenerateSerializer]
internal readonly struct TypedSuccessSurrogate<T>(SuccessKind kind, T value)
{
    [Id(0)] public SuccessKind Kind { get; } = kind;
    [Id(1)] public T Value { get; } = value;
}

[RegisterConverter]
internal sealed class TypedSuccessSurrogateConverter<T> : IConverter<Success<T>, TypedSuccessSurrogate<T>>
{
    public Success<T> ConvertFromSurrogate(in TypedSuccessSurrogate<T> surrogate)
    {
        return new Success<T>(surrogate.Kind, surrogate.Value);
    }

    public TypedSuccessSurrogate<T> ConvertToSurrogate(in Success<T> value)
    {
        return new TypedSuccessSurrogate<T>(value.Kind, value.Value);
    }
}