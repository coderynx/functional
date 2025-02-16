namespace Coderynx.Functional.Option;

public readonly struct ValueOption<T> where T : struct
{
    private readonly T? _value;

    private ValueOption(T? value)
    {
        _value = value;
    }

    public static ValueOption<T> Some(T value)
    {
        return new ValueOption<T>(value);
    }

    public static ValueOption<T> None()
    {
        return new ValueOption<T>(null);
    }

    public TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none)
    {
        return _value.HasValue ? some(_value.Value) : none();
    }

    public T ValueOr(Func<T> valueProvider)
    {
        return _value ?? valueProvider();
    }
}