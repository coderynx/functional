using Coderynx.Functional.Results;
using Coderynx.Functional.Results.Errors;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Coderynx.Functional.WebApi.Tests;

public sealed class ResultExtensionsTests
{
    [Fact]
    public void ToHttpResult_SuccessResult_Created_Returns201()
    {
        // Arrange
        var result = Result.Created();

        // Act
        var actionResult = result.ToHttpResult();

        // Assert
        Assert.IsType<Ok>(actionResult);

        var createdResult = (Ok)actionResult;
        Assert.Equal(200, createdResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_SuccessResult_Updated_Returns200()
    {
        // Arrange
        var result = Result.Updated();

        // Act
        var actionResult = result.ToHttpResult();

        // Assert
        Assert.IsType<NoContent>(actionResult);

        var okResult = (NoContent)actionResult;
        Assert.Equal(204, okResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_SuccessResult_Deleted_Returns204()
    {
        // Arrange
        var result = Result.Deleted();

        // Act
        var actionResult = result.ToHttpResult();

        // Assert
        Assert.IsType<NoContent>(actionResult);

        var okResult = (NoContent)actionResult;
        Assert.Equal(204, okResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_SuccessResult_Found_Returns200()
    {
        // Arrange
        var result = Result.Found();

        // Act
        var actionResult = result.ToHttpResult();

        // Assert
        Assert.IsType<Ok>(actionResult);

        var okResult = (Ok)actionResult;
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_SuccessResult_Accepted_Returns202()
    {
        // Arrange
        var result = Result.Accepted();

        // Act
        var actionResult = result.ToHttpResult();

        // Assert
        Assert.IsType<Accepted>(actionResult);

        var acceptedResult = (Accepted)actionResult;
        Assert.Equal(202, acceptedResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_ErrorResult_NotFound_Returns404()
    {
        // Arrange
        Result result = Error.NotFound("E001", "Resource not found");

        // Act
        var actionResult = result.ToHttpResult();

        // Assert
        Assert.IsType<ProblemHttpResult>(actionResult);

        var problemResult = (ProblemHttpResult)actionResult;
        Assert.Equal(404, problemResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_ErrorResult_InvalidInput_Returns400()
    {
        // Arrange
        Result result = Error.InvalidInput("E002", "Invalid input");

        // Act
        var actionResult = result.ToHttpResult();

        // Assert
        Assert.IsType<ProblemHttpResult>(actionResult);

        var problemResult = (ProblemHttpResult)actionResult;
        Assert.Equal(400, problemResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_ErrorResult_Conflict_Returns409()
    {
        // Arrange
        Result result = Error.Conflict("E005", "Conflict occurred");

        // Act
        var actionResult = result.ToHttpResult();

        // Assert
        Assert.IsType<ProblemHttpResult>(actionResult);

        var problemResult = (ProblemHttpResult)actionResult;
        Assert.Equal(409, problemResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_ErrorResult_Unexpected_Returns500()
    {
        // Arrange
        Result result = Error.Unexpected("E006", "Unexpected error");

        // Act
        var actionResult = result.ToHttpResult();

        // Assert
        Assert.IsType<ProblemHttpResult>(actionResult);

        var problemResult = (ProblemHttpResult)actionResult;
        Assert.Equal(500, problemResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_TypedSuccessResult_Created_ReturnsCreatedWithValue()
    {
        // Arrange
        var testValue = "test-value";
        var result = Result.Created(testValue);

        // Act
        var actionResult = result.ToHttpResult();

        // Assert
        Assert.IsType<Ok<object>>(actionResult);

        var okResult = (Ok<object>)actionResult;
        Assert.Equal(testValue, okResult.Value);
    }
    
    [Fact]
    public void ToHttpResult_ShouldReturnCreatedWithValue_WhenValueTransformationIsProvided()
    {
        // Arrange
        var testValue = "test-value";
        var result = Result.Created(testValue);

        // Act
        var actionResult = result.ToHttpResult(value => $"{value}1");

        // Assert
        Assert.IsType<Ok<object>>(actionResult);

        var okResult = (Ok<object>)actionResult;
        Assert.Equal($"{testValue}1", okResult.Value);
    }

    [Fact]
    public void ToHttpResult_TypedSuccessResult_Found_ReturnsOkWithValue()
    {
        // Arrange
        var testValue = new { Id = 1, Name = "Test" };
        var result = Result.Found(testValue);

        // Act
        var actionResult = result.ToHttpResult();

        // Assert
        Assert.IsType<Ok<object>>(actionResult);

        var okResult = (Ok<object>)actionResult;
        Assert.Equal(testValue, okResult.Value);
    }

    [Fact]
    public void ToHttpResult_TypedErrorResult_NotFound_ReturnsNotFoundWithMessage()
    {
        // Arrange
        Result result = Error.NotFound("E001", "Resource not found");

        // Act
        var actionResult = result.ToHttpResult();

        // Assert
        Assert.IsType<ProblemHttpResult>(actionResult);

        var problemResult = (ProblemHttpResult)actionResult;
        Assert.Equal(404, problemResult.StatusCode);
        Assert.Equal("E001", problemResult.ProblemDetails.Title);
        Assert.Equal("Resource not found", problemResult.ProblemDetails.Detail);
    }

    [Fact]
    public void ToHttpResult_TypedErrorResult_InvalidInput_ReturnsBadRequestWithMessage()
    {
        // Arrange
        Result result = Error.InvalidInput("E002", "Invalid input provided");

        // Act
        var actionResult = result.ToHttpResult();

        // Assert
        Assert.IsType<ProblemHttpResult>(actionResult);

        var problemResult = (ProblemHttpResult)actionResult;
        Assert.Equal(400, problemResult.StatusCode);
        Assert.Equal("E002", problemResult.ProblemDetails.Title);
        Assert.Equal("Invalid input provided", problemResult.ProblemDetails.Detail);
    }
}