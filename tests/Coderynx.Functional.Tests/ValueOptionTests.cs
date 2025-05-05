using Coderynx.Functional.Options;

namespace Coderynx.Functional.Tests;

public sealed class ValueOptionTests
{
    [Fact]
    public void Some_ShouldCreateValueOptionWithNonNullValue()
    {
        var option = ValueOption.Some(5);

        Assert.Equal(5, option.Match(value => value, () => -1));
    }

    [Fact]
    public void None_ShouldCreateValueOptionWithNullValue()
    {
        var option = ValueOption.None<int>();

        Assert.Equal(-1, option.Match(value => value, () => -1));
    }

    [Fact]
    public void Match_ShouldReturnSomeValue_WhenOptionIsSome()
    {
        var option = ValueOption.Some(5);

        var result = option.Match(value => value * 2, () => -1);

        Assert.Equal(10, result);
    }

    [Fact]
    public void Match_ShouldReturnNoneValue_WhenOptionIsNone()
    {
        var option = ValueOption.None<int>();

        var result = option.Match(value => value * 2, () => -1);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void ValueOr_ShouldReturnValue_WhenOptionIsSome()
    {
        var option = ValueOption.Some(5);

        var value = option.ValueOr(() => -1);

        Assert.Equal(5, value);
    }

    [Fact]
    public void ValueOr_ShouldReturnProvidedValue_WhenOptionIsNone()
    {
        var option = ValueOption.None<int>();

        var value = option.ValueOr(() => -1);

        Assert.Equal(-1, value);
    }

    [Fact]
    public void ValueOrNull_ShouldReturnValue_WhenOptionIsSome()
    {
        var option = ValueOption.Some(5);

        var value = option.ValueOrNull();

        Assert.Equal(5, value);
    }

    [Fact]
    public void ValueOrNull_ShouldReturnNull_WhenOptionIsNone()
    {
        var option = ValueOption.None<int>();

        var value = option.ValueOrNull();

        Assert.Null(value);
    }

    [Fact]
    public void ImplicitOperator_FromNonNullNullable_ShouldCreateSomeOption()
    {
        int? value = 5;
        ValueOption<int> option = value;

        Assert.True(option.IsSome);
        Assert.Equal(5, option.Match(v => v, () => -1));
    }

    [Fact]
    public void ImplicitOperator_FromNullNullable_ShouldCreateNoneOption()
    {
        int? value = null;
        ValueOption<int> option = value;

        Assert.True(option.IsNone);
        Assert.Equal(-1, option.Match(v => v, () => -1));
    }

    [Fact]
    public void ExplicitOperator_FromSomeOption_ShouldReturnNonNullValue()
    {
        var option = ValueOption.Some(5);
        var value = (int?)option;

        Assert.Equal(5, value);
    }

    [Fact]
    public void ExplicitOperator_FromNoneOption_ShouldReturnNullValue()
    {
        var option = ValueOption.None<int>();
        var value = (int?)option;

        Assert.Null(value);
    }
}