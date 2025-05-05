namespace Coderynx.Functional.Options;

/// <summary>
/// Provides a base class for implementing the Option monad pattern,
/// representing a container that may or may not contain a value.
/// </summary>
public abstract class Option
{
    /// <summary>
    /// Creates an Option containing a value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to wrap in an Option.</param>
    /// <returns>An Option containing the provided value.</returns>
    public static Option<T> Some<T>(T value) where T : class
    {
        return new Option<T>(value);
    }

    /// <summary>
    /// Creates an empty Option (None).
    /// </summary>
    /// <typeparam name="T">The type parameter for the Option.</typeparam>
    /// <returns>An empty Option of the specified type.</returns>
    public static Option<T> None<T>() where T : class
    {
        return new Option<T>(null);
    }
}

/// <summary>
/// Represents an Option that may contain a value of type T or no value (None).
/// This is a type-safe alternative to using null references.
/// </summary>
/// <typeparam name="T">The type of the contained value.</typeparam>
public sealed class Option<T> : Option where T : class
{
    private readonly T? _value;

    /// <summary>
    /// Initializes a new instance of the Option class.
    /// </summary>
    /// <param name="value">The value to be contained in this Option.</param>
    internal Option(T? value)
    {
        _value = value;
    }

    /// <summary>
    /// Gets a value indicating whether this Option contains a value.
    /// </summary>
    public bool IsSome => _value is not null;

    /// <summary>
    /// Gets a value indicating whether this Option does not contain a value.
    /// </summary>
    public bool IsNone => !IsSome;

    /// <summary>
    /// Transforms the value in this Option using the provided mapping function.
    /// </summary>
    /// <typeparam name="TOut">The type of the output value.</typeparam>
    /// <param name="map">The function to apply to the contained value.</param>
    /// <returns>A new Option containing the transformed value, or None if this Option is None.</returns>
    public Option<TOut> Map<TOut>(Func<T, TOut> map) where TOut : class
    {
        return _value is not null ? Some(map(_value)) : None<TOut>();
    }

    /// <summary>
    /// Transforms the value in this Option to a value type using the provided mapping function.
    /// </summary>
    /// <typeparam name="TOut">The value type of the output.</typeparam>
    /// <param name="map">The function to apply to the contained value.</param>
    /// <returns>A new ValueOption containing the transformed value, or None if this Option is None.</returns>
    public ValueOption<TOut> MapValue<TOut>(Func<T, TOut> map) where TOut : struct
    {
        return _value is not null ? ValueOption.Some(map(_value)) : ValueOption.None<TOut>();
    }

    /// <summary>
    /// Chains together multiple operations that return Options.
    /// </summary>
    /// <typeparam name="TOut">The type of the output Option.</typeparam>
    /// <param name="bind">The function to apply to the contained value.</param>
    /// <returns>The result of applying the function to the contained value, or None if this Option is None.</returns>
    public Option<TOut> Bind<TOut>(Func<T, Option<TOut>> bind) where TOut : class
    {
        return _value is not null ? bind(_value) : None<TOut>();
    }

    /// <summary>
    /// Applies one of two functions based on whether this Option contains a value.
    /// </summary>
    /// <typeparam name="TOut">The return type of both functions.</typeparam>
    /// <param name="some">The function to apply if this Option contains a value.</param>
    /// <param name="none">The function to apply if this Option is None.</param>
    /// <returns>The result of the applied function.</returns>
    public TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none)
    {
        return _value is not null ? some(_value) : none();
    }

    /// <summary>
    /// Performs one of two actions based on whether this Option contains a value.
    /// </summary>
    /// <param name="some">The action to perform if this Option contains a value.</param>
    /// <param name="none">The action to perform if this Option is None.</param>
    public void Match(Action<T> some, Action none)
    {
        if (_value is not null)
        {
            some(_value);
        }
        else
        {
            none();
        }
    }

    /// <summary>
    /// Applies a predicate to the contained value and returns an Option based on the result.
    /// </summary>
    /// <param name="filter">The predicate to apply to the value.</param>
    /// <returns>This Option if it contains a value that satisfies the predicate; otherwise, None.</returns>
    public Option<T> Filter(Func<T, bool> filter)
    {
        return _value is not null && filter(_value) ? Some(_value) : None<T>();
    }

    /// <summary>
    /// Returns the contained value or throws an exception if this Option is None.
    /// </summary>
    /// <returns>The contained value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Option is None.</exception>
    public T ValueOrThrow()
    {
        return _value ?? throw new InvalidOperationException("The value not present");
    }

    /// <summary>
    /// Returns the contained value or a default value provided by the given function.
    /// </summary>
    /// <param name="valueProvider">The function that provides a default value.</param>
    /// <returns>The contained value if present; otherwise, the result of the valueProvider function.</returns>
    public T ValueOr(Func<T> valueProvider)
    {
        return _value ?? valueProvider();
    }

    /// <summary>
    /// Returns the contained value or null if this Option is None.
    /// </summary>
    /// <returns>The contained value or null.</returns>
    public T? ValueOrNull()
    {
        return _value;
    }

    /// <summary>
    /// Implicitly converts a value to an Option.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>An Option containing the value, or None if the value is null.</returns>
    public static implicit operator Option<T>(T? value)
    {
        return value is not null ? Some(value) : None<T>();
    }

    /// <summary>
    /// Explicitly converts an Option to its contained value or null.
    /// </summary>
    /// <param name="option">The Option to convert.</param>
    /// <returns>The contained value or null if the Option is None.</returns>
    public static explicit operator T?(Option<T> option)
    {
        return option.IsSome ? option.ValueOrThrow() : null;
    }
}