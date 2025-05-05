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
            onSuccess: async () =>
            {
                await Task.Delay(1);
                return Success.Custom();
            }
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
            onFinally: async () =>
            {
                await Task.Delay(1);
                finallyCalled = true;
            }
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
            onSuccess: async value =>
            {
                await Task.Delay(1);
                return Success.Custom(value);
            }
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
            onSuccess: async value =>
            {
                await Task.Delay(1);
                return Success.Custom(value);
            },
            onCatch: async exception =>
            {
                await Task.Delay(1);
                return Error.InvalidInput("E400", exception.Message);
            }
        );

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
            onFinally: async () =>
            {
                await Task.Delay(1);
                finallyCalled = true;
            }
        );

        // Assert
        Assert.True(finallyCalled);
    }
}