namespace Coderynx.Functional.Result;

public class Result
{
    protected Result(bool isSuccess, Error error, ResultSuccess? successType = null)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
                throw new InvalidOperationException();
            case false when error == Error.None:
                throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
        SuccessType = successType;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    public ResultSuccess? SuccessType { get; }

    public static Result Success(ResultSuccess resultSuccess)
    {
        return new Result(true, Error.None, resultSuccess);
    }

    public static Result<TValue> Success<TValue>(TValue value, ResultSuccess resultSuccess)
    {
        return new Result<TValue>(value, true, Error.None, resultSuccess);
    }

    public static Result Updated()
    {
        return Success(ResultSuccess.Updated);
    }

    public static Result Created()
    {
        return Success(ResultSuccess.Created);
    }

    public static Result<TValue> Created<TValue>(TValue value)
    {
        return Success(value, ResultSuccess.Created);
    }

    public static Result<TValue> Found<TValue>(TValue value)
    {
        return Success(value, ResultSuccess.Found);
    }

    public static Result Accepted()
    {
        return Success(ResultSuccess.Accepted);
    }

    public static Result Deleted()
    {
        return Success(ResultSuccess.Deleted);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }

    public static Result<TValue> Failure<TValue>(Error error)
    {
        return new Result<TValue>(default, false, error);
    }

    public TOutput Match<TOutput>(Func<TOutput> success, Func<Error, TOutput> fail)
    {
        return IsFailure
            ? fail(Error)
            : success();
    }

    public static implicit operator Result(Error error)
    {
        return Failure(error);
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error, ResultSuccess? successType = null)
        : base(isSuccess, error, successType)
    {
        _value = value;
    }

    public bool HasValue => IsSuccess && _value is not null;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public TOutput Match<TOutput>(Func<TValue, TOutput> success, Func<Error, TOutput> fail)
    {
        return IsFailure
            ? fail(Error)
            : success(Value);
    }

    public static implicit operator Result<TValue>(Error error)
    {
        return Failure<TValue>(error);
    }
}