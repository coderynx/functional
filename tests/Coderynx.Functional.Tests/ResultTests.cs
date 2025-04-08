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
    public void Bind_ShouldReturnNextResult_WhenCurrentResultIsSuccess()
    {
        // Arrange
        var initialResult = Result.Success(ResultSuccess.Created);

        // Act
        var finalResult = initialResult.Bind(() => Result.Success(ResultSuccess.Updated));

        // Assert
        finalResult.IsSuccess.Should().BeTrue();
        finalResult.SuccessType.Should().Be(ResultSuccess.Updated);
    }

    [Fact]
    public void Bind_ShouldPropagateFailure_WhenCurrentResultIsFailure()
    {
        // Arrange
        var initialResult = Result.Failure(new Error(ResultError.InvalidInput, "Invalid", "Invalid"));

        // Act
        var finalResult = initialResult.Bind(() => Result.Success(ResultSuccess.Updated));

        // Assert
        finalResult.IsSuccess.Should().BeFalse();
        finalResult.Error.Should().Be(initialResult.Error);
    }

    [Fact]
    public void Bind_WithValue_ShouldReturnNextResult_WhenCurrentResultIsSuccess()
    {
        // Arrange
        var initialResult = Result.Success("InitialValue", ResultSuccess.Created);

        // Act
        var finalResult = initialResult.Bind(value => Result.Success(value + "Updated", ResultSuccess.Updated));

        // Assert
        finalResult.IsSuccess.Should().BeTrue();
        finalResult.Value.Should().Be("InitialValueUpdated");
        finalResult.SuccessType.Should().Be(ResultSuccess.Updated);
    }

    [Fact]
    public void Bind_WithValue_ShouldPropagateFailure_WhenCurrentResultIsFailure()
    {
        // Arrange
        var initialResult = Result.Failure<string>(new Error(ResultError.InvalidInput, "Invalid", "Invalid"));

        // Act
        var finalResult = initialResult.Bind(value => Result.Success(value + "Updated", ResultSuccess.Updated));

        // Assert
        finalResult.IsSuccess.Should().BeFalse();
        finalResult.Error.Should().Be(initialResult.Error);
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