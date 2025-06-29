﻿using Coderynx.Functional.Results.Errors;
using Coderynx.Functional.Results.Successes;

namespace Coderynx.Functional.Results;

/// <summary>
///     Represents the result of an operation that can either succeed or fail.
///     Provides a functional approach to error handling without using exceptions for control flow.
/// </summary>
public class Result
{
    internal Result(Success success)
    {
        Error = Error.None;
        Success = success;
    }

    internal Result(Error error)
    {
        Error = error;
        Success = Success.None;
    }

    /// <summary>
    ///     Gets the error associated with this result, or <see cref="Error.None" /> if this is a success result.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    ///     Gets the success object associated with this result, or <see cref="Success.None" /> if this is an error result.
    /// </summary>
    public Success Success { get; }

    /// <summary>
    ///     Gets a value indicating whether this result represents a successful operation.
    /// </summary>
    public bool IsSuccess => Success != Success.None && Error == Error.None;

    /// <summary>
    ///     Gets a value indicating whether this result represents a failed operation.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    ///     Creates a success result representing a resource creation operation.
    /// </summary>
    /// <returns>A success result with <see cref="SuccessKind.Created" />.</returns>
    public static Result Created()
    {
        return new Result(new Success(SuccessKind.Created));
    }

    /// <summary>
    ///     Creates a success result with a value representing a resource creation operation.
    /// </summary>
    /// <typeparam name="TValue">The type of the created value.</typeparam>
    /// <param name="value">The created value.</param>
    /// <returns>A success result with the created value.</returns>
    public static Result<TValue> Created<TValue>(TValue value)
    {
        return new Result<TValue>(new Success<TValue>(SuccessKind.Created, value));
    }

    /// <summary>
    ///     Creates a success result representing a resource update operation.
    /// </summary>
    /// <returns>A success result with <see cref="SuccessKind.Updated" />.</returns>
    public static Result Updated()
    {
        return new Result(new Success(SuccessKind.Updated));
    }

    /// <summary>
    ///     Creates a success result with a value representing a resource update operation.
    /// </summary>
    /// <typeparam name="TValue">The type of the updated value.</typeparam>
    /// <param name="value">The updated value.</param>
    /// <returns>A success result with the updated value.</returns>
    public static Result<TValue> Updated<TValue>(TValue value)
    {
        return new Result<TValue>(new Success<TValue>(SuccessKind.Updated, value));
    }

    /// <summary>
    ///     Creates a success result representing a resource retrieval operation.
    /// </summary>
    /// <returns>A success result with <see cref="SuccessKind.Found" />.</returns>
    public static Result Found()
    {
        return new Result(new Success(SuccessKind.Found));
    }

    /// <summary>
    ///     Creates a success result with a value representing a resource retrieval operation.
    /// </summary>
    /// <typeparam name="TValue">The type of the found value.</typeparam>
    /// <param name="value">The found value.</param>
    /// <returns>A success result with the found value.</returns>
    public static Result<TValue> Found<TValue>(TValue value)
    {
        return new Result<TValue>(new Success<TValue>(SuccessKind.Found, value));
    }

    /// <summary>
    ///     Creates a success result representing an acceptance operation.
    /// </summary>
    /// <returns>A success result with <see cref="SuccessKind.Accepted" />.</returns>
    public static Result Accepted()
    {
        return new Success(SuccessKind.Accepted);
    }

    /// <summary>
    ///     Creates a success result with a value representing an acceptance operation.
    /// </summary>
    /// <typeparam name="TValue">The type of the accepted value.</typeparam>
    /// <param name="value">The accepted value.</param>
    /// <returns>A success result with the accepted value.</returns>
    public static Result<TValue> Accepted<TValue>(TValue value)
    {
        return new Result<TValue>(new Success<TValue>(SuccessKind.Accepted, value));
    }

    /// <summary>
    ///     Creates a success result representing a deletion operation.
    /// </summary>
    /// <returns>A success result with <see cref="SuccessKind.Deleted" />.</returns>
    public static Result Deleted()
    {
        return new Result(new Success(SuccessKind.Deleted));
    }

    /// <summary>
    ///     Creates a success result with a value representing a deletion operation.
    /// </summary>
    /// <typeparam name="TValue">The type of the deleted value.</typeparam>
    /// <param name="value">The deleted value.</param>
    /// <returns>A success result with the deleted value.</returns>
    public static Result<TValue> Deleted<TValue>(TValue value)
    {
        return new Result<TValue>(new Success<TValue>(SuccessKind.Deleted, value));
    }

    /// <summary>
    ///     Executes an action within a try-catch block and returns a Result.
    /// </summary>
    /// <param name="onTry">The action to execute within the try block.</param>
    /// <param name="onSuccess">
    ///     Optional function to create a custom Success object when the action succeeds.
    ///     If not provided, a default Success object is created.
    /// </param>
    /// <param name="onCatch">
    ///     Optional function to convert exceptions into Error objects.
    ///     If not provided, exceptions are converted to an Unexpected error.
    /// </param>
    /// <param name="onFinally">Optional action to execute in the finally block.</param>
    /// <returns>A Result object representing the outcome of the operation.</returns>
    public static Result TryCatch(
        Action onTry,
        Func<Success>? onSuccess = null,
        Func<Exception, Error>? onCatch = null,
        Action? onFinally = null)
    {
        try
        {
            onTry();

            var success = onSuccess?.Invoke() ?? Success.Custom();
            return new Result(success);
        }
        catch (ErrorException exception)
        {
            return exception.Error;
        }
        catch (Exception exception)
        {
            return onCatch?.Invoke(exception) ?? Error.Unexpected(exception);
        }
        finally
        {
            onFinally?.Invoke();
        }
    }

    /// <summary>
    ///     Executes an asynchronous action within a try-catch block and returns a Result.
    /// </summary>
    /// <param name="onTry">The asynchronous action to execute within the try block.</param>
    /// <param name="onSuccess">
    ///     Optional function to create a custom Success object when the action succeeds.
    ///     If not provided, a default Success object is created.
    /// </param>
    /// <param name="onCatch">
    ///     Optional function to convert exceptions into Error objects.
    ///     If not provided, exceptions are converted to an Unexpected error.
    /// </param>
    /// <param name="onFinally">Optional action to execute in the finally block.</param>
    /// <returns>A Result object representing the outcome of the operation.</returns>
    public static async Task<Result> TryCatchAsync(
        Func<Task> onTry,
        Func<Success>? onSuccess = null,
        Func<Exception, Error>? onCatch = null,
        Action? onFinally = null)
    {
        try
        {
            await onTry();

            var success = onSuccess?.Invoke() ?? Success.Custom();
            return new Result(success);
        }
        catch (ErrorException exception)
        {
            return exception.Error;
        }
        catch (Exception exception)
        {
            return onCatch?.Invoke(exception) ?? Error.Unexpected(exception);
        }
        finally
        {
            onFinally?.Invoke();
        }
    }

    /// <summary>
    ///     Executes a function within a try-catch block and returns a Result with the function's return value.
    /// </summary>
    /// <typeparam name="T">The type of value returned by the function.</typeparam>
    /// <param name="onTry">The function to execute within the try block.</param>
    /// <param name="onSuccess">
    ///     Optional function to create a custom Success object with the value when the function succeeds.
    ///     If not provided, a default Success object with the value is created.
    /// </param>
    /// <param name="onCatch">
    ///     Optional function to convert exceptions into Error objects.
    ///     If not provided, exceptions are converted to an Unexpected error.
    /// </param>
    /// <param name="onFinally">Optional action to execute in the finally block.</param>
    /// <returns>A Result object containing the value or an error representing the outcome of the operation.</returns>
    public static Result<T> TryCatch<T>(
        Func<T> onTry,
        Func<T, Success<T>>? onSuccess = null,
        Func<Exception, Error>? onCatch = null,
        Action? onFinally = null)
    {
        try
        {
            var value = onTry();

            // TODO: Evaluate the possibility to check if value is null.
            var result = onSuccess?.Invoke(value) ?? Success.Custom(value);
            return new Result<T>(result);
        }
        catch (ErrorException exception)
        {
            return new Error(
                kind: exception.Error.Kind,
                code: exception.Error.Code,
                message: exception.Error.Message);
        }
        catch (Exception exception)
        {
            return onCatch?.Invoke(exception) ?? Error.Unexpected(exception);
        }
        finally
        {
            onFinally?.Invoke();
        }
    }

    /// <summary>
    ///     Executes an asynchronous function within a try-catch block and returns a Result with the function's return value.
    /// </summary>
    /// <typeparam name="T">The type of value returned by the function.</typeparam>
    /// <param name="onTry">The asynchronous function to execute within the try block.</param>
    /// <param name="onSuccess">
    ///     Optional function to create a custom Success object with the value when the function succeeds.
    ///     If not provided, a default Success object with the value is created.
    /// </param>
    /// <param name="onCatch">
    ///     Optional function to convert exceptions into Error objects.
    ///     If not provided, exceptions are converted to an Unexpected error.
    /// </param>
    /// <param name="onFinally">Optional action to execute in the finally block.</param>
    /// <returns>A Result object containing the value or an error representing the outcome of the operation.</returns>
    public static async Task<Result<T>> TryCatchAsync<T>(
        Func<Task<T>> onTry,
        Func<T, Success<T>>? onSuccess = null,
        Func<Exception, Error>? onCatch = null,
        Action? onFinally = null)
    {
        try
        {
            var value = await onTry();

            var result = onSuccess?.Invoke(value) ?? Success.Custom(value);
            return new Result<T>(result);
        }
        catch (ErrorException exception)
        {
            return new Error(
                kind: exception.Error.Kind,
                code: exception.Error.Code,
                message: exception.Error.Message);
        }
        catch (Exception exception)
        {
            return onCatch?.Invoke(exception) ?? Error.Unexpected(exception);
        }
        finally
        {
            onFinally?.Invoke();
        }
    }

    public static implicit operator Result(Success success)
    {
        return new Result(success);
    }

    public static implicit operator Result(Error error)
    {
        return new Result(error);
    }
}