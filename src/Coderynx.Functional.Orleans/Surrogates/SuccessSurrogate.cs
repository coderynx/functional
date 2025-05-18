using Coderynx.Functional.Results.Successes;
using Orleans;

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
        return surrogate.Kind switch
        {
            SuccessKind.None => Success.None,
            SuccessKind.Found => Success.Found(),
            SuccessKind.Created => Success.Created(),
            SuccessKind.Updated => Success.Updated(),
            SuccessKind.Deleted => Success.Deleted(),
            SuccessKind.Accepted => Success.Accepted(),
            SuccessKind.Custom => Success.Custom(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public SuccessSurrogate ConvertToSurrogate(in Success value)
    {
        return new SuccessSurrogate(value.Kind);
    }
}