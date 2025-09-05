using Coderynx.Functional.Options;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Coderynx.Functional.WebApi.Tests;

public sealed class OptionExtensionsTests
{
    [Fact]
    public void ToHttpResult_SomeOption_ReturnsOkWithValue()
    {
        // Arrange
        var testValue = "test-value";
        var option = Option.Some(testValue);

        // Act
        var actionResult = option.ToHttpResult();

        // Assert
        Assert.IsType<Ok<string>>(actionResult);

        var okResult = (Ok<string>)actionResult;
        Assert.Equal(testValue, okResult.Value);
    }

    [Fact]
    public void ToHttpResult_NoneOption_ReturnsNotFound()
    {
        // Arrange
        var option = Option.None<string>();

        // Act
        var actionResult = option.ToHttpResult();

        // Assert
        Assert.IsType<ProblemHttpResult>(actionResult);

        var problemResult = (ProblemHttpResult)actionResult;
        Assert.Equal(404, problemResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_WithCustomTransform_ReturnsOkWithValue()
    {
        // Arrange
        var testValue = "test-value";
        var option = Option.Some(testValue);

        // Act
        var actionResult = option.ToHttpResult(value => $"{value}1");

        // Assert
        Assert.IsType<Ok<object>>(actionResult);

        var okResult = (Ok<object>)actionResult;
        Assert.Equal($"{testValue}1", okResult.Value);
    }


    [Fact]
    public void ToHttpResult_WithCustomHandlers_NoneOption_CallsOnNone()
    {
        // Arrange
        var option = Option.None<string>();

        // Act
        var actionResult = option.ToHttpResult(value => $"{value}1");

        // Assert
        Assert.IsType<ProblemHttpResult>(actionResult);

        var problemResult = (ProblemHttpResult)actionResult;
        Assert.Equal(404, problemResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_NullValue_NoneOption_ReturnsNotFound()
    {
        // Arrange
        var option = Option.None<object>();

        // Act
        var actionResult = option.ToHttpResult();

        // Assert
        Assert.IsType<ProblemHttpResult>(actionResult);

        var problemResult = (ProblemHttpResult)actionResult;
        Assert.Equal(404, problemResult.StatusCode);
    }
}