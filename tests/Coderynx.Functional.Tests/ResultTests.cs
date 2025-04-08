using Coderynx.Functional.Results;
using FluentAssertions;

namespace Coderynx.Functional.Tests;

public sealed class ResultTests
{
    private static Error TestError => new(ResultError.InvalidInput, nameof(TestError), nameof(TestError));

    [Fact]
    public void SuccessResult_ShouldHaveNoError()
    {
        // Arrange
        var expectedError = Error.None;

        // Act
        var result = Result.Success(ResultSuccess.Created);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(expectedError);
    }

    [Fact]
    public void FailureResult_ShouldHaveError()
    {
        // Arrange
        var expectedError = TestError;

        // Act
        var result = Result.Failure(expectedError);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(expectedError);
    }

    [Fact]
    public void SuccessResultWithValue_ShouldHaveValue()
    {
        // Arrange
        const string expectedValue = "TestValue";

        // Act
        var result = Result.Success(expectedValue, ResultSuccess.Created);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.HasValue.Should().BeTrue();
        result.Value.Should().Be(expectedValue);
    }

    [Fact]
    public void FailureResultWithValue_ShouldNotHaveValue()
    {
        // Arrange
        var expectedError = TestError;

        // Act
        var result = Result.Failure<string>(expectedError);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasValue.Should().BeFalse();
        result.Invoking(r => r.Value).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Match_ShouldReturnCorrectOutputBasedOnResult()
    {
        // Arrange
        var successResult = Result.Created("TestValue");
        var failureResult = Result.Failure<string>(TestError);

        // Act
        var successOutput = successResult.Match(() => "Success", _ => "Failure");
        var failureOutput = failureResult.Match(() => "Success", _ => "Failure");

        // Assert
        successOutput.Should().Be("Success");
        failureOutput.Should().Be("Failure");
    }

    [Fact]
    public void Bind_ShouldReturnNewResult_WhenResultIsSuccess()
    {
        // Arrange
        var initialResult = Result.Success(ResultSuccess.Created);

        // Act
        var boundResult = initialResult.Bind(() => Result.Success(ResultSuccess.Updated));

        // Assert
        boundResult.IsSuccess.Should().BeTrue();
        boundResult.SuccessType.Should().Be(ResultSuccess.Updated);
    }

    [Fact]
    public void Bind_ShouldPropagateFailure_WhenResultIsFailure()
    {
        // Arrange
        var initialResult = Result.Failure(TestError);

        // Act
        var boundResult = initialResult.Bind(() => Result.Success(ResultSuccess.Updated));

        // Assert
        boundResult.IsSuccess.Should().BeFalse();
        boundResult.Error.Should().Be(TestError);
    }

    [Fact]
    public void Bind_WithGeneric_ShouldReturnNewResult_WhenResultIsSuccess()
    {
        // Arrange
        var initialResult = Result.Success(42, ResultSuccess.Created);

        // Act
        var boundResult = initialResult.Bind(() => Result.Success("Success", ResultSuccess.Updated));

        // Assert
        boundResult.IsSuccess.Should().BeTrue();
        boundResult.Value.Should().Be("Success");
        boundResult.SuccessType.Should().Be(ResultSuccess.Updated);
    }

    [Fact]
    public void Bind_WithGeneric_ShouldPropagateFailure_WhenResultIsFailure()
    {
        // Arrange
        var initialResult = Result.Failure<int>(TestError);

        // Act
        var boundResult = initialResult.Bind(() => Result.Success("Success", ResultSuccess.Updated));

        // Assert
        boundResult.IsSuccess.Should().BeFalse();
        boundResult.Error.Should().Be(TestError);
    }
    
    [Fact]
    public void ImplicitConversion_ShouldReturnFailureResult()
    {
        // Arrange
        var expectedError = TestError;

        // Act
        Result result = expectedError;

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(expectedError);
    }
}