using Coderynx.Functional.Results;
using Coderynx.Functional.Results.Errors;
using Coderynx.Functional.Results.Successes;

namespace Coderynx.Functional.Tests;

public sealed class ResultTests
{
    [Fact]
    public void Found_ReturnsSuccessResultWithFoundKind()
    {
        var result = Result.Found();
        Assert.True(result.IsSuccess);
        Assert.Equal(SuccessKind.Found, result.Success.Kind);
    }

    [Fact]
    public void Found_ReturnsSuccessValuedResultWithFoundKind()
    {
        // Arrange & Act
        var valueResult = Result.Found(123);

        // Assert
        Assert.True(valueResult.IsSuccess);
        Assert.Equal(SuccessKind.Found, ((Result)valueResult).Success.Kind);
        Assert.Equal(123, valueResult.Value);
    }

    [Fact]
    public void Updated_ReturnsSuccessResultWithUpdatedKind()
    {
        // Arrange & Act
        var result = Result.Updated();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(SuccessKind.Updated, result.Success.Kind);
    }

    [Fact]
    public void Updated_ReturnsSuccessValuedResultWithUpdatedKind()
    {
        // Arrange & Act
        var result = Result.Updated("value");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(SuccessKind.Updated, ((Result)result).Success.Kind);
        Assert.Equal("value", result.Value);
    }

    [Fact]
    public void Deleted_ReturnsSuccessResultWithDeletedKind()
    {
        // Arrange & Act
        var result = Result.Deleted();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(SuccessKind.Deleted, result.Success.Kind);
    }

    [Fact]
    public void Deleted_ReturnsSuccessValuedResultWithDeletedKind()
    {
        // Arrange & Act
        var result = Result.Deleted("value");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(SuccessKind.Deleted, ((Result)result).Success.Kind);
        Assert.Equal("value", result.Value);
    }

    [Fact]
    public void Accepted_ReturnsSuccessResultWithAcceptedKind()
    {
        // Arrange & Act
        var result = Result.Accepted();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(SuccessKind.Accepted, result.Success.Kind);
    }

    [Fact]
    public void Accepted_ReturnsSuccessValuedResultWithAcceptedKind()
    {
        // Arrange & Act
        var result = Result.Accepted("value");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(SuccessKind.Accepted, ((Result)result).Success.Kind);
        Assert.Equal("value", result.Value);
    }

    [Fact]
    public void Created_ReturnsSuccessResultWithCreatedKind()
    {
        // Arrange & Act
        var result = Result.Created();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(SuccessKind.Created, result.Success.Kind);
    }

    [Fact]
    public void Created_ReturnsSuccessValuedResultWithCreatedKind()
    {
        // Arrange & Act
        var result = Result.Created("new");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(SuccessKind.Created, ((Result)result).Success.Kind);
        Assert.Equal("new", result.Value);
    }

    [Fact]
    public void TryCatch_Action_Success()
    {
        // Arrange & Act
        var result = Result.TryCatch(() => { });

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(SuccessKind.Custom, result.Success.Kind);
    }

    [Fact]
    public void TryCatch_Action_ErrorException()
    {
        // Arrange
        var error = Error.InvalidInput("E001", "Invalid");

        // Act
        var result = Result.TryCatch(
            onTry: () => throw error,
            onSuccess: Success.Custom
        );

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void TryCatch_Action_UnexpectedException()
    {
        // Arrange & Act
        var result = Result.TryCatch(() => throw new InvalidOperationException("fail"));

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorKind.Unexpected, result.Error.Kind);
        Assert.Contains("fail", result.Error.Message);
    }

    [Fact]
    public void TryCatch_Action_FinallyActionIsCalled()
    {
        // Arrange
        var finallyCalled = false;

        // Act
        Result.TryCatch(() => { }, onFinally: () => finallyCalled = true);

        // Assert
        Assert.True(finallyCalled);
    }

    [Fact]
    public void TryCatch_FuncT_Success()
    {
        // Arrange & Act
        var result = Result.TryCatch(
            onTry: int () => 1,
            onSuccess: Success.Custom
        );

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public void TryCatch_FuncT_ErrorException()
    {
        // Arrange
        var error = Error.Conflict("E409", "Conflict");

        // Act
        var result = Result.TryCatch(
            onTry: int () => throw error,
            onSuccess: Success.Custom,
            onCatch: exception => Error.InvalidInput("E400", exception.Message));

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void TryCatch_FuncT_UnexpectedException()
    {
        // Arrange & Act
        var result = Result.TryCatch(() => throw new Exception("boom"));

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorKind.Unexpected, result.Error.Kind);
        Assert.Contains("boom", result.Error.Message);
    }

    [Fact]
    public void TryCatch_FuncT_FinallyActionIsCalled()
    {
        // Arrange
        var finallyCalled = false;

        // Act
        Result.TryCatch(
            onTry: () => { },
            onFinally: () => finallyCalled = true);

        // Assert
        Assert.True(finallyCalled);
    }

    [Fact]
    public async Task TryCatchAsync_Action_Success()
    {
        // Arrange & Act
        var result = await Result.TryCatchAsync(async () => await Task.CompletedTask);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(SuccessKind.Custom, result.Success.Kind);
    }

    [Fact]
    public async Task TryCatchAsync_Action_ErrorException()
    {
        // Arrange
        var error = Error.InvalidInput("E001", "Invalid");

        // Act
        var result = await Result.TryCatchAsync(
            onTry: async () =>
            {
                await Task.Delay(1);
                throw error;
            },
            onSuccess: Success.Custom
        );

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public async Task TryCatchAsync_Action_UnexpectedException()
    {
        // Arrange & Act
        var result = await Result.TryCatchAsync(async () =>
        {
            await Task.Delay(1);
            throw new InvalidOperationException("fail");
        });

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorKind.Unexpected, result.Error.Kind);
        Assert.Contains("fail", result.Error.Message);
    }

    [Fact]
    public async Task TryCatchAsync_Action_FinallyActionIsCalled()
    {
        // Arrange
        var finallyCalled = false;

        // Act
        await Result.TryCatchAsync(
            onTry: async () => await Task.CompletedTask,
            onFinally: () => finallyCalled = true
        );

        // Assert
        Assert.True(finallyCalled);
    }

    [Fact]
    public async Task TryCatchAsync_FuncT_Success()
    {
        // Arrange & Act
        var result = await Result.TryCatchAsync(
            onTry: async () =>
            {
                await Task.Delay(1);
                return 1;
            },
            onSuccess: Success.Custom
        );

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public async Task TryCatchAsync_FuncT_ErrorException()
    {
        // Arrange
        var error = Error.Conflict("E409", "Conflict");

        // Act
        var result = await Result.TryCatchAsync<int>(
            onTry: async () =>
            {
                await Task.Delay(1);
                throw error;
            },
            onSuccess: Success.Custom,
            onCatch: exception => Error.InvalidInput("E400", exception.Message));

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public async Task TryCatchAsync_FuncT_UnexpectedException()
    {
        // Arrange & Act
        var result = await Result.TryCatchAsync<int>(async () =>
        {
            await Task.Delay(1);
            throw new Exception("boom");
        });

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorKind.Unexpected, result.Error.Kind);
        Assert.Contains("boom", result.Error.Message);
    }

    [Fact]
    public async Task TryCatchAsync_FuncT_FinallyActionIsCalled()
    {
        // Arrange
        var finallyCalled = false;

        // Act
        await Result.TryCatchAsync(
            onTry: async () =>
            {
                await Task.Delay(1);
                return 42;
            },
            onFinally: () => finallyCalled = true
        );

        // Assert
        Assert.True(finallyCalled);
    }

    [Fact]
    public void ImplicitConversion_FromSuccessT_ToResultT()
    {
        // Arrange
        var success = Success.Custom("value");

        // Act
        Result<string> result = success;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(success, result.Success);
        Assert.Equal("value", result.Value);
    }

    [Fact]
    public void Map_TransformsValue_WhenSuccessful()
    {
        var result = Result.Created(5);
        var mapped = result.Map(x => x * 2);

        Assert.True(mapped.IsSuccess);
        Assert.Equal(10, mapped.Value);
    }

    [Fact]
    public void Flatten_UnwrapsNestedResult_WhenSuccessful()
    {
        var inner = Result.Created("flattened");
        var outer = Result.Created(inner);
        var flattened = outer.Flatten<string>();

        Assert.True(flattened.IsSuccess);
        Assert.Equal("flattened", flattened.Value);
    }

    [Fact]
    public void Flatten_PreservesError_WhenOuterFails()
    {
        var error = Error.NotFound("E404", "Missing");
        Result<Result<string>> outer = new Result<Result<string>>(error);
        var flattened = outer.Flatten<string>();

        Assert.True(flattened.IsFailure);
        Assert.Equal(error, flattened.Error);
    }

    [Fact]
    public async Task TapAsync_PerformsSideEffect_WhenSuccess()
    {
        var called = false;
        var result = Result.Created("tap");
        var tapped = await result.TapAsync(async _ =>
        {
            await Task.Delay(1);
            called = true;
        });

        Assert.True(tapped.IsSuccess);
        Assert.True(called);
        Assert.Equal("tap", tapped.Value);
    }

    [Fact]
    public async Task TapAsync_DoesNotPerformSideEffect_WhenFailure()
    {
        var called = false;
        var error = Error.InvalidInput("E001", "Invalid");
        var result = new Result<string>(error);
        var tapped = await result.TapAsync(async _ =>
        {
            await Task.Delay(1);
            called = true;
        });

        Assert.True(tapped.IsFailure);
        Assert.False(called);
        Assert.Equal(error, tapped.Error);
    }

    [Fact]
    public async Task MapAsync_TransformsValue_WhenSuccess()
    {
        var result = Result.Created(10);
        var mapped = await result.MapAsync(async val =>
        {
            await Task.Delay(1);
            return val * 3;
        });

        Assert.True(mapped.IsSuccess);
        Assert.Equal(30, mapped.Value);
    }

    [Fact]
    public async Task MapAsync_PreservesError_WhenFailure()
    {
        var error = Error.Conflict("E409", "Conflict");
        var result = new Result<int>(error);
        var mapped = await result.MapAsync(async val =>
        {
            await Task.Delay(1);
            return val * 2;
        });

        Assert.True(mapped.IsFailure);
        Assert.Equal(error, mapped.Error);
    }

    [Fact]
    public void Tap_PerformsSideEffect_WhenSuccess()
    {
        var called = false;
        var result = Result.Created("ok");
        var tapped = result.Tap(_ => called = true);

        Assert.True(tapped.IsSuccess);
        Assert.True(called);
        Assert.Equal("ok", tapped.Value);
    }

    [Fact]
    public void Tap_DoesNotPerformSideEffect_WhenFailure()
    {
        var called = false;
        var result = new Result<string>(Error.InvalidInput("E001", "Invalid"));
        var tapped = result.Tap(_ => called = true);

        Assert.True(tapped.IsFailure);
        Assert.False(called);
    }
}