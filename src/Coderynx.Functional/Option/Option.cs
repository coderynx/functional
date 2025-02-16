namespace Coderynx.Functional.Option;

public sealed class Option<T> where T : class
{
    private readonly T? _value;

    private Option(T? value) => _value = value;

    public bool IsSome => _value is not null;

    public static Option<T> Some(T value) => new(value);

    public static Option<T> None() => new(null);

    public Option<TOut> Map<TOut>(Func<T, TOut> map) where TOut : class =>
        _value is not null ? Option<TOut>.Some(map(_value)) : Option<TOut>.None();

    public ValueOption<TOut> MapValue<TOut>(Func<T, TOut> map) where TOut : struct =>
        _value is not null ? ValueOption<TOut>.Some(map(_value)) : ValueOption<TOut>.None();

    public Option<TOut> Bind<TOut>(Func<T, Option<TOut>> bind) where TOut : class =>
        _value is not null ? bind(_value) : Option<TOut>.None();

    public TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none) =>
        _value is not null ? some(_value) : none();

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

    public Option<T> Filter(Func<T, bool> filter) => _value is not null && filter(_value) ? Some(_value) : None();

    public T ValueOrThrow() => _value ?? throw new InvalidOperationException("The value not present");

    public T ValueOr(Func<T> valueProvider) => _value ?? valueProvider();
}