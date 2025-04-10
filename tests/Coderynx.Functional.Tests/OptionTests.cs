using Coderynx.Functional.Options;
using FluentAssertions;

namespace Coderynx.Functional.Tests;

public sealed class OptionTests
{
    [Fact]
    public void Some_ShouldCreateOptionWithNonNullValue()
    {
        var option = Option<string>.Some("TestValue");

        option.IsSome.Should().BeTrue();
        option.ValueOrThrow().Should().Be("TestValue");
    }

    [Fact]
    public void None_ShouldCreateOptionWithNullValue()
    {
        var option = Option<string>.None();

        option.IsSome.Should().BeFalse();
        option.Invoking(o => o.ValueOrThrow()).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Map_ShouldTransformValue_WhenOptionIsSome()
    {
        var option = Option<string>.Some("TestValue");
        var mappedOption = option.Map(value => value.ToUpper());

        mappedOption.IsSome.Should().BeTrue();
        mappedOption.ValueOrThrow().Should().Be("TESTVALUE");
    }

    [Fact]
    public void Map_ShouldReturnNone_WhenOptionIsNone()
    {
        var option = Option<string>.None();
        var mappedOption = option.Map(value => value.ToUpper());

        mappedOption.IsSome.Should().BeFalse();
    }

    [Fact]
    public void Bind_ShouldReturnTransformedOption_WhenOptionIsSome()
    {
        var option = Option<string>.Some("TestValue");
        var boundOption = option.Bind(value => Option<string>.Some(value.Length.ToString()));

        boundOption.IsSome.Should().BeTrue();
        boundOption.ValueOrThrow().Should().Be("9");
    }

    [Fact]
    public void Bind_ShouldReturnNone_WhenOptionIsNone()
    {
        var option = Option<string>.None();
        var boundOption = option.Bind(value => Option<string>.Some(value.Length.ToString()));

        boundOption.IsSome.Should().BeFalse();
    }

    [Fact]
    public void Match_ShouldReturnCorrectValueBasedOnOption()
    {
        var someOption = Option<string>.Some("TestValue");
        var noneOption = Option<string>.None();

        var someResult = someOption.Match(value => value.ToUpper(), () => "NONE");
        var noneResult = noneOption.Match(value => value.ToUpper(), () => "NONE");

        someResult.Should().Be("TESTVALUE");
        noneResult.Should().Be("NONE");
    }

    [Fact]
    public void Filter_ShouldReturnSome_WhenPredicateIsTrue()
    {
        var option = Option<string>.Some("TestValue");
        var filteredOption = option.Filter(value => value.Length > 5);

        filteredOption.IsSome.Should().BeTrue();
        filteredOption.ValueOrThrow().Should().Be("TestValue");
    }

    [Fact]
    public void Filter_ShouldReturnNone_WhenPredicateIsFalse()
    {
        var option = Option<string>.Some("TestValue");
        var filteredOption = option.Filter(value => value.Length < 5);

        filteredOption.IsSome.Should().BeFalse();
    }

    [Fact]
    public async Task BindAsync_ShouldReturnTransformedOption_WhenOptionIsSome()
    {
        var option = Option<string>.Some("TestValue");
        var boundOption = await option.BindAsync(async value =>
        {
            await Task.Delay(10);
            return Option<string>.Some(value.ToUpper());
        });

        boundOption.IsSome.Should().BeTrue();
        boundOption.ValueOrThrow().Should().Be("TESTVALUE");
    }

    [Fact]
    public async Task BindAsync_ShouldReturnNone_WhenOptionIsNone()
    {
        var option = Option<string>.None();
        var boundOption = await option.BindAsync(async value =>
        {
            await Task.Delay(10);
            return Option<string>.Some(value.ToUpper());
        });

        boundOption.IsSome.Should().BeFalse();
    }

    [Fact]
    public void ValueOr_ShouldReturnValue_WhenOptionIsSome()
    {
        var option = Option<string>.Some("TestValue");
        var value = option.ValueOr(() => "DefaultValue");

        value.Should().Be("TestValue");
    }

    [Fact]
    public void ValueOr_ShouldReturnDefaultValue_WhenOptionIsNone()
    {
        var option = Option<string>.None();
        var value = option.ValueOr(() => "DefaultValue");

        value.Should().Be("DefaultValue");
    }

    [Fact]
    public void ValueOrNull_ShouldReturnValue_WhenOptionIsSome()
    {
        var option = Option<string>.Some("TestValue");
        var value = option.ValueOrNull();

        value.Should().Be("TestValue");
    }

    [Fact]
    public void ValueOrNull_ShouldReturnNull_WhenOptionIsNone()
    {
        var option = Option<string>.None();
        var value = option.ValueOrNull();

        value.Should().BeNull();
    }
}