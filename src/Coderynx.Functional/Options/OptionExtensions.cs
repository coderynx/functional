namespace Coderynx.Functional.Options;

public static class OptionExtensions
{
    /// <summary>
    ///     Chains the current option with an asynchronous operation that depends on the value.
    /// </summary>
    /// <typeparam name="T">The type of the value in the current option.</typeparam>
    /// <typeparam name="TOut">The type of the value in the resulting option.</typeparam>
    /// <param name="option">The current option.</param>
    /// <param name="bind">The asynchronous function to execute if the option contains a value.</param>
    /// <returns>A task representing the resulting <see cref="Option{TOut}" /> or propagates None.</returns>
    public static async Task<Option<TOut>> BindAsync<T, TOut>(this Option<T> option, Func<T, Task<Option<TOut>>> bind)
        where T : class where TOut : class
    {
        return option.IsSome ? await bind(option.ValueOrThrow()) : Option.None<TOut>();
    }
}