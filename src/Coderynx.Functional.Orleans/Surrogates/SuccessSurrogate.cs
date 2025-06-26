using Coderynx.Functional.Results.Successes;

namespace Coderynx.Functional.Orleans.Surrogates;

[GenerateSerializer]
internal readonly struct SuccessSurrogate(SuccessKind kind)
{
    [Id(0)] public SuccessKind Kind { get; } = kind;
}

[RegisterConverter]
internal sealed class SuccessSurrogateConverter : IConverter<Success, SuccessSurrogate>
{
    public Success ConvertFromSurrogate(in SuccessSurrogate surrogate)
    {
        return new Success(surrogate.Kind);
    }

    public SuccessSurrogate ConvertToSurrogate(in Success value)
    {
        return new SuccessSurrogate(value.Kind);
    }
}