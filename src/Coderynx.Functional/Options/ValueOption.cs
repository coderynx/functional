namespace Coderynx.Functional.Options;

/// <summary>
/// Provides factory methods for creating <see cref="ValueOption{T}"/> instances.
/// </summary>
public static class ValueOption
{
    /// <summary>
    /// Creates a <see cref="ValueOption{T}"/> instance containing the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value to wrap, must be a value type.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <returns>A <see cref="ValueOption{T}"/> containing the value.</returns>
    public static ValueOption<T> Some<T>(T value) where T : struct
    {
        return new ValueOption<T>(value);
    }

    /// <summary>
    /// Creates an empty <see cref="ValueOption{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">The type parameter for the option, must be a value type.</typeparam>
    /// <returns>An empty <see cref="ValueOption{T}"/>.</returns>
    public static ValueOption<T> None<T>() where T : struct
    {
        return new ValueOption<T>(null);
    }
}

/// <summary>
/// Represents an optional value of type <typeparamref name="T"/>.
/// This is a value type equivalent of Option, specifically designed for value types.
/// </summary>
/// <typeparam name="T">The type of the optional value, must be a value type.</typeparam>
public readonly struct ValueOption<T> where T : struct
{
    private readonly T? _value;

    /// <summary>
    /// Gets a value indicating whether this instance contains a value.
    /// </summary>
    public bool IsSome => _value.HasValue;

    /// <summary>
    /// Gets a value indicating whether this instance does not contain a value.
    /// </summary>
    public bool IsNone => !IsSome;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueOption{T}"/> struct.
    /// </summary>
    /// <param name="value">The nullable value to wrap.</param>
    internal ValueOption(T? value)
    {
        _value = value;
    }

    /// <summary>
    /// Applies one of the provided functions based on whether this instance contains a value.
    /// </summary>
    /// <typeparam name="TOut">The type of the return value.</typeparam>
    /// <param name="some">The function to apply if a value is present.</param>
    /// <param name="none">The function to apply if no value is present.</param>
    /// <returns>
    /// The result of applying <paramref name="some"/> to the contained value if present;
    /// otherwise, the result of the <paramref name="none"/> function.
    /// </returns>
    public TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none)
    {
        return _value.HasValue
            ? some(_value.Value)
            : none();
    }

    /// <summary>
    /// Returns the contained value if present; otherwise, returns the result of the provided function.
    /// </summary>
    /// <param name="valueProvider">A function that provides a default value when no value is present.</param>
    /// <returns>The contained value if present; otherwise, the result of <paramref name="valueProvider"/>.</returns>
    public T ValueOr(Func<T> valueProvider)
    {
        return _value ?? valueProvider();
    }

    /// <summary>
    /// Returns the contained value as a nullable type.
    /// </summary>
    /// <returns>The contained value if present; otherwise, null.</returns>
    public T? ValueOrNull()
    {
        return _value;
    }

    /// <summary>
    /// Implicitly converts a nullable value to a <see cref="ValueOption{T}"/>.
    /// </summary>
    /// <param name="value">The nullable value to convert.</param>
    public static implicit operator ValueOption<T>(T? value)
    {
        return value.HasValue
            ? ValueOption.Some(value.Value)
            : ValueOption.None<T>();
    }

    /// <summary>
    /// Explicitly converts a <see cref="ValueOption{T}"/> to a nullable value.
    /// </summary>
    /// <param name="option">The option to convert.</param>
    public static explicit operator T?(ValueOption<T> option)
    {
        return option.IsSome
            ? option._value
            : null;
    }
}