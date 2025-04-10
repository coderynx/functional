namespace Coderynx.Functional.Options;

/// <summary>
///     Represents an optional value that can either contain a value of type <typeparamref name="T" /> or be empty (None).
///     This struct is designed for value types (struct).
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public readonly struct ValueOption<T> where T : struct
{
    private readonly T? _value;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ValueOption{T}" /> struct.
    /// </summary>
    /// <param name="value">The value to wrap, or null to represent None.</param>
    private ValueOption(T? value)
    {
        _value = value;
    }

    /// <summary>
    ///     Creates a <see cref="ValueOption{T}" /> that contains a value.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <returns>A <see cref="ValueOption{T}" /> containing the specified value.</returns>
    public static ValueOption<T> Some(T value)
    {
        return new ValueOption<T>(value);
    }

    /// <summary>
    ///     Creates a <see cref="ValueOption{T}" /> that represents None (no value).
    /// </summary>
    /// <returns>A <see cref="ValueOption{T}" /> representing None.</returns>
    public static ValueOption<T> None()
    {
        return new ValueOption<T>(null);
    }

    /// <summary>
    ///     Matches the current state of the option and executes the corresponding function.
    /// </summary>
    /// <typeparam name="TOut">The return type of the match functions.</typeparam>
    /// <param name="some">The function to execute if the option contains a value.</param>
    /// <param name="none">The function to execute if the option represents None.</param>
    /// <returns>The result of the executed function.</returns>
    public TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none)
    {
        return _value.HasValue ? some(_value.Value) : none();
    }

    /// <summary>
    ///     Chains the current option with another function that returns a new option.
    /// </summary>
    /// <typeparam name="TOut">The type of the value in the resulting option.</typeparam>
    /// <param name="bind">The function to transform the value into another option.</param>
    /// <returns>The resulting option, or an empty option if the original option is empty.</returns>
    public async Task<ValueOption<TOut>> BindAsync<TOut>(Func<T, Task<ValueOption<TOut>>> bind) where TOut : struct
    {
        return _value.HasValue ? await bind(_value.Value) : ValueOption<TOut>.None();
    }

    /// <summary>
    ///     Returns the value if present, or the result of the provided value provider function if None.
    /// </summary>
    /// <param name="valueProvider">A function that provides a value if the option is None.</param>
    /// <returns>The value of the option, or the result of the value provider function.</returns>
    public T ValueOr(Func<T> valueProvider)
    {
        return _value ?? valueProvider();
    }

    /// <summary>
    ///     Returns the value if present, or null if None.
    /// </summary>
    /// <returns>The value of the option, or null if None.</returns>
    public T? ValueOrNull()
    {
        return _value;
    }
}