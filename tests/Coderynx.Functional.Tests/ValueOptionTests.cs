using Coderynx.Functional.Option;
using FluentAssertions;

namespace Coderynx.Functional.Tests;

public sealed class ValueOptionTests
{
    [Fact]
    public void Some_ShouldCreateValueOptionWithNonNullValue()
    {
        var option = ValueOption<int>.Some(5);

        option.Match(value => value, () => -1).Should().Be(5);
    }

    [Fact]
    public void None_ShouldCreateValueOptionWithNullValue()
    {
        var option = ValueOption<int>.None();

        option.Match(value => value, () => -1).Should().Be(-1);
    }

    [Fact]
    public void Match_ShouldReturnSomeValue_WhenOptionIsSome()
    {
        var option = ValueOption<int>.Some(5);

        var result = option.Match(value => value * 2, () => -1);

        result.Should().Be(10);
    }

    [Fact]
    public void Match_ShouldReturnNoneValue_WhenOptionIsNone()
    {
        var option = ValueOption<int>.None();

        var result = option.Match(value => value * 2, () => -1);

        result.Should().Be(-1);
    }

    [Fact]
    public void ValueOr_ShouldReturnValue_WhenOptionIsSome()
    {
        var option = ValueOption<int>.Some(5);

        var value = option.ValueOr(() => -1);

        value.Should().Be(5);
    }

    [Fact]
    public void ValueOr_ShouldReturnProvidedValue_WhenOptionIsNone()
    {
        var option = ValueOption<int>.None();

        var value = option.ValueOr(() => -1);

        value.Should().Be(-1);
    }

    [Fact]
    public void ValueOrNull_ShouldReturnValue_WhenOptionIsSome()
    {
        var option = ValueOption<int>.Some(5);

        var value = option.ValueOrNull();

        value.Should().Be(5);
    }

    [Fact]
    public void ValueOrNull_ShouldReturnNull_WhenOptionIsNone()
    {
        var option = ValueOption<int>.None();

        var value = option.ValueOrNull();

        value.Should().BeNull();
    }
}