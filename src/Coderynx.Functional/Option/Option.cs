namespace Coderynx.Functional.Option;

/// <summary>
///     Represents an optional value that may or may not contain a value of type <typeparamref name="T" />.
/// </summary>
/// <typeparam name="T">The type of the value contained in the option. Must be a reference type.</typeparam>
public sealed class Option<T> where T : class
{
    private readonly T? _value;

    /// <summary>
    ///     Private constructor to initialize the option with a value or null.
    /// </summary>
    /// <param name="value">The value to store, or null if the option is empty.</param>
    private Option(T? value)
    {
        _value = value;
    }

    /// <summary>
    ///     Indicates whether the option contains a value.
    /// </summary>
    public bool IsSome => _value is not null;

    /// <summary>
    ///     Creates an option containing a value.
    /// </summary>
    /// <param name="value">The value to store in the option.</param>
    /// <returns>An option containing the specified value.</returns>
    public static Option<T> Some(T value)
    {
        return new Option<T>(value);
    }

    /// <summary>
    ///     Creates an empty option.
    /// </summary>
    /// <returns>An empty option.</returns>
    public static Option<T> None()
    {
        return new Option<T>(null);
    }

    /// <summary>
    ///     Transforms the value inside the option using the specified mapping function.
    /// </summary>
    /// <typeparam name="TOut">The type of the transformed value.</typeparam>
    /// <param name="map">The function to transform the value.</param>
    /// <returns>An option containing the transformed value, or an empty option if the original option is empty.</returns>
    public Option<TOut> Map<TOut>(Func<T, TOut> map) where TOut : class
    {
        return _value is not null ? Option<TOut>.Some(map(_value)) : Option<TOut>.None();
    }

    /// <summary>
    ///     Transforms the value inside the option into a value type using the specified mapping function.
    /// </summary>
    /// <typeparam name="TOut">The value type of the transformed value.</typeparam>
    /// <param name="map">The function to transform the value.</param>
    /// <returns>
    ///     A <see cref="ValueOption{TOut}" /> containing the transformed value, or an empty option if the original option
    ///     is empty.
    /// </returns>
    public ValueOption<TOut> MapValue<TOut>(Func<T, TOut> map) where TOut : struct
    {
        return _value is not null ? ValueOption<TOut>.Some(map(_value)) : ValueOption<TOut>.None();
    }

    /// <summary>
    ///     Transforms the value inside the option into another option using the specified binding function.
    /// </summary>
    /// <typeparam name="TOut">The type of the value in the resulting option.</typeparam>
    /// <param name="bind">The function to transform the value into another option.</param>
    /// <returns>The resulting option, or an empty option if the original option is empty.</returns>
    public Option<TOut> Bind<TOut>(Func<T, Option<TOut>> bind) where TOut : class
    {
        return _value is not null ? bind(_value) : Option<TOut>.None();
    }

    /// <summary>
    ///     Matches the option to one of two functions based on whether it contains a value.
    /// </summary>
    /// <typeparam name="TOut">The return type of the match functions.</typeparam>
    /// <param name="some">The function to execute if the option contains a value.</param>
    /// <param name="none">The function to execute if the option is empty.</param>
    /// <returns>The result of the executed function.</returns>
    public TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none)
    {
        return _value is not null ? some(_value) : none();
    }

    /// <summary>
    ///     Matches the option to one of two actions based on whether it contains a value.
    /// </summary>
    /// <param name="some">The action to execute if the option contains a value.</param>
    /// <param name="none">The action to execute if the option is empty.</param>
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
    ///     Filters the option based on a predicate function.
    /// </summary>
    /// <param name="filter">The predicate function to test the value.</param>
    /// <returns>The original option if the predicate is true, or an empty option otherwise.</returns>
    public Option<T> Filter(Func<T, bool> filter)
    {
        return _value is not null && filter(_value) ? Some(_value) : None();
    }

    /// <summary>
    ///     Retrieves the value inside the option or throws an exception if the option is empty.
    /// </summary>
    /// <returns>The value inside the option.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the option is empty.</exception>
    public T ValueOrThrow()
    {
        return _value ?? throw new InvalidOperationException("The value not present");
    }

    /// <summary>
    ///     Retrieves the value inside the option or provides a fallback value using a function if the option is empty.
    /// </summary>
    /// <param name="valueProvider">The function to provide a fallback value.</param>
    /// <returns>The value inside the option, or the fallback value.</returns>
    public T ValueOr(Func<T> valueProvider)
    {
        return _value ?? valueProvider();
    }

    /// <summary>
    ///     Retrieves the value inside the option or null if the option is empty.
    /// </summary>
    /// <returns>The value inside the option, or null.</returns>
    public T? ValueOrNull()
    {
        return _value;
    }
}