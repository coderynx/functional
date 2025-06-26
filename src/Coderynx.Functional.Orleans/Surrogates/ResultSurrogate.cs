using Coderynx.Functional.Results;
using Coderynx.Functional.Results.Errors;
using Coderynx.Functional.Results.Successes;

namespace Coderynx.Functional.Orleans.Surrogates;

[GenerateSerializer]
internal readonly struct ResultSurrogate(bool isSuccess, Error error, Success success)
{
    [Id(0)] public bool IsSuccess { get; } = isSuccess;
    [Id(1)] public Error Error { get; } = error;
    [Id(2)] public Success Success { get; } = success;
}

[RegisterConverter]
internal sealed class ResultSurrogateConverter : IConverter<Result, ResultSurrogate>
{
    public Result ConvertFromSurrogate(in ResultSurrogate surrogate)
    {
        return surrogate.IsSuccess
            ? surrogate.Success
            : surrogate.Error;
    }

    public ResultSurrogate ConvertToSurrogate(in Result value)
    {
        return new ResultSurrogate(value.IsSuccess, value.Error, value.Success);
    }
}