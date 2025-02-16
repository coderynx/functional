using Coderynx.Functional;
using Coderynx.Functional.Result;
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
        var result = Functional.Result.Result.Success(ResultSuccess.Created);

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
        var result = Functional.Result.Result.Failure(expectedError);

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
        var result = Functional.Result.Result.Success(expectedValue, ResultSuccess.Created);

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
        var result = Functional.Result.Result.Failure<string>(expectedError);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasValue.Should().BeFalse();
        result.Invoking(r => r.Value).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Match_ShouldReturnCorrectOutputBasedOnResult()
    {
        // Arrange
        var successResult = Functional.Result.Result.Created("TestValue");
        var failureResult = Functional.Result.Result.Failure<string>(TestError);

        // Act
        var successOutput = successResult.Match(() => "Success", _ => "Failure");
        var failureOutput = failureResult.Match(() => "Success", _ => "Failure");

        // Assert
        successOutput.Should().Be("Success");
        failureOutput.Should().Be("Failure");
    }

    [Fact]
    public void ImplicitConversion_ShouldReturnFailureResult()
    {
        // Arrange
        var expectedError = TestError;

        // Act
        Functional.Result.Result result = expectedError;

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(expectedError);
    }
}