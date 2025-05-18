namespace Coderynx.Functional.Results.Successes;

/// <summary>
///     Represents a success result that can be used in functional result patterns.
///     Provides static methods to create different types of success results.
/// </summary>
public record Success
{
    /// <summary>
    ///     A default success instance with None kind.
    /// </summary>
    public static readonly Success None = new(SuccessKind.None);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Success" /> class with the specified kind.
    /// </summary>
    /// <param name="Kind">The kind of success result.</param>
    public Success(SuccessKind Kind)
    {
        this.Kind = Kind;
    }

    /// <summary>
    ///     Gets the kind of success result.
    /// </summary>
    public SuccessKind Kind { get; }

    /// <summary>
    ///     Creates a success result indicating a resource was found.
    /// </summary>
    /// <returns>A success result with Found kind.</returns>
    public static Success Found()
    {
        return new Success(SuccessKind.Found);
    }

    /// <summary>
    ///     Creates a success result with a value indicating a resource was found.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value associated with the success result.</param>
    /// <returns>A success result with Found kind and the specified value.</returns>
    public static Success<T> Found<T>(T value)
    {
        return new Success<T>(SuccessKind.Found, value);
    }

    /// <summary>
    ///     Creates a success result indicating a resource was created.
    /// </summary>
    /// <returns>A success result with Created kind.</returns>
    public static Success Created()
    {
        return new Success(SuccessKind.Created);
    }

    /// <summary>
    ///     Creates a success result with a value indicating a resource was created.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value associated with the success result.</param>
    /// <returns>A success result with Created kind and the specified value.</returns>
    public static Success<T> Created<T>(T value)
    {
        return new Success<T>(SuccessKind.Created, value);
    }

    /// <summary>
    ///     Creates a success result indicating a resource was updated.
    /// </summary>
    /// <returns>A success result with Updated kind.</returns>
    public static Success Updated()
    {
        return new Success(SuccessKind.Updated);
    }

    /// <summary>
    ///     Creates a success result with a value indicating a resource was updated.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value associated with the success result.</param>
    /// <returns>A success result with Updated kind and the specified value.</returns>
    public static Success<T> Updated<T>(T value)
    {
        return new Success<T>(SuccessKind.Updated, value);
    }

    /// <summary>
    ///     Creates a success result indicating a resource was deleted.
    /// </summary>
    /// <returns>A success result with Deleted kind.</returns>
    public static Success Deleted()
    {
        return new Success(SuccessKind.Deleted);
    }

    /// <summary>
    ///     Creates a success result with a value indicating a resource was deleted.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value associated with the success result.</param>
    /// <returns>A success result with Deleted kind and the specified value.</returns>
    public static Success<T> Deleted<T>(T value)
    {
        return new Success<T>(SuccessKind.Deleted, value);
    }

    /// <summary>
    ///     Creates a success result indicating a request was accepted.
    /// </summary>
    /// <returns>A success result with Accepted kind.</returns>
    public static Success Accepted()
    {
        return new Success(SuccessKind.Accepted);
    }

    /// <summary>
    ///     Creates a success result with a value indicating a request was accepted.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value associated with the success result.</param>
    /// <returns>A success result with Accepted kind and the specified value.</returns>
    public static Success<T> Accepted<T>(T value)
    {
        return new Success<T>(SuccessKind.Accepted, value);
    }

    /// <summary>
    ///     Creates a success result with Custom kind.
    /// </summary>
    /// <returns>A success result with Custom kind.</returns>
    public static Success Custom()
    {
        return new Success(SuccessKind.Custom);
    }

    /// <summary>
    ///     Creates a success result with a value and Custom kind.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value associated with the success result.</param>
    /// <returns>A success result with Custom kind and the specified value.</returns>
    public static Success<T> Custom<T>(T value)
    {
        return new Success<T>(SuccessKind.Custom, value);
    }
}

/// <summary>
///     Represents a success result that contains a value of type <typeparamref name="T" />.
///     Inherits from <see cref="Success" /> and adds the ability to store a strongly-typed value.
/// </summary>
/// <typeparam name="T">The type of the value associated with the success result.</typeparam>
public sealed record Success<T> : Success
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Success{T}" /> class with the specified kind and value.
    /// </summary>
    /// <param name="Kind">The kind of success result.</param>
    /// <param name="Value">The value associated with the success result.</param>
    public Success(SuccessKind Kind, T Value) : base(Kind)
    {
        this.Value = Value;
    }

    /// <summary>
    ///     Gets the value associated with the success result.
    /// </summary>
    public T Value { get; }
}