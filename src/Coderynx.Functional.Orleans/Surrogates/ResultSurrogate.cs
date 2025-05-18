using Coderynx.Functional.Results;
using Coderynx.Functional.Results.Errors;
using Coderynx.Functional.Results.Successes;
using Orleans;

namespace Coderynx.Functional.Orleans.Surrogates;

[GenerateSerializer]
internal readonly struct ResultSurrogate(bool isSuccess, bool isFailure, Error error, Success success)
{
    [Id(0)] public bool IsSuccess { get; } = isSuccess;
    [Id(1)] public bool IsFailure { get; } = isFailure;
    [Id(2)] public Error Error { get; } = error;
    [Id(3)] public Success Success { get; } = success;
}

[RegisterConverter]
internal sealed class ResultSurrogateConverter : IConverter<Result, ResultSurrogate>
{
    public Result ConvertFromSurrogate(in ResultSurrogate surrogate)
    {
        return surrogate.IsFailure
            ? surrogate.Error
            : surrogate.Success;
    }

    public ResultSurrogate ConvertToSurrogate(in Result value)
    {
        return new ResultSurrogate(value.IsSuccess, value.IsFailure, value.Error, value.Success);
    }
}