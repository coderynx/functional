using Coderynx.Functional.Options;

namespace Coderynx.Functional.Tests;

public sealed class OptionTests
{
    [Fact]
    public void Some_ShouldCreateOptionWithNonNullValue()
    {
        var option = Option.Some("TestValue");

        Assert.True(option.IsSome);
        Assert.Equal("TestValue", option.ValueOrThrow());
    }

    [Fact]
    public void None_ShouldCreateOptionWithNullValue()
    {
        var option = Option.None<string>();

        Assert.False(option.IsSome);
        Assert.Throws<InvalidOperationException>(() => option.ValueOrThrow());
    }

    [Fact]
    public void Map_ShouldTransformValue_WhenOptionIsSome()
    {
        var option = Option.Some<string>("TestValue");
        var mappedOption = option.Map(value => value.ToUpper());

        Assert.True(mappedOption.IsSome);
        Assert.Equal("TESTVALUE", mappedOption.ValueOrThrow());
    }

    [Fact]
    public void Map_ShouldReturnNone_WhenOptionIsNone()
    {
        var option = Option.None<string>();
        var mappedOption = option.Map(value => value.ToUpper());

        Assert.False(mappedOption.IsSome);
    }

    [Fact]
    public void Bind_ShouldReturnTransformedOption_WhenOptionIsSome()
    {
        var option = Option.Some<string>("TestValue");
        var boundOption = option.Bind(value => Option.Some(value.Length.ToString()));

        Assert.True(boundOption.IsSome);
        Assert.Equal("9", boundOption.ValueOrThrow());
    }

    [Fact]
    public void Bind_ShouldReturnNone_WhenOptionIsNone()
    {
        var option = Option.None<string>();
        var boundOption = option.Bind(value => Option.Some(value.Length.ToString()));

        Assert.False(boundOption.IsSome);
    }

    [Fact]
    public void Match_ShouldReturnCorrectValueBasedOnOption()
    {
        var someOption = Option.Some("TestValue");
        var noneOption = Option.None<string>();

        var someResult = someOption.Match(value => value.ToUpper(), () => "NONE");
        var noneResult = noneOption.Match(value => value.ToUpper(), () => "NONE");

        Assert.Equal("TESTVALUE", someResult);
        Assert.Equal("NONE", noneResult);
    }

    [Fact]
    public void Filter_ShouldReturnSome_WhenPredicateIsTrue()
    {
        var option = Option.Some<string>("TestValue");
        var filteredOption = option.Filter(value => value.Length > 5);

        Assert.True(filteredOption.IsSome);
        Assert.Equal("TestValue", filteredOption.ValueOrThrow());
    }

    [Fact]
    public void Filter_ShouldReturnNone_WhenPredicateIsFalse()
    {
        var option = Option.Some<string>("TestValue");
        var filteredOption = option.Filter(value => value.Length < 5);

        Assert.False(filteredOption.IsSome);
    }

    [Fact]
    public async Task BindAsync_ShouldReturnTransformedOption_WhenOptionIsSome()
    {
        var option = Option.Some<string>("TestValue");
        var boundOption = await option.BindAsync(async value =>
        {
            await Task.Delay(10);
            return Option.Some(value.ToUpper());
        });

        Assert.True(boundOption.IsSome);
        Assert.Equal("TESTVALUE", boundOption.ValueOrThrow());
    }

    [Fact]
    public async Task BindAsync_ShouldReturnNone_WhenOptionIsNone()
    {
        var option = Option.None<string>();
        var boundOption = await option.BindAsync(async value =>
        {
            await Task.Delay(10);
            return Option.Some(value.ToUpper());
        });

        Assert.False(boundOption.IsSome);
    }

    [Fact]
    public void ValueOr_ShouldReturnValue_WhenOptionIsSome()
    {
        var option = Option.Some<string>("TestValue");
        var value = option.ValueOr(() => "DefaultValue");

        Assert.Equal("TestValue", value);
    }

    [Fact]
    public void ValueOr_ShouldReturnDefaultValue_WhenOptionIsNone()
    {
        var option = Option.None<string>();
        var value = option.ValueOr(() => "DefaultValue");

        Assert.Equal("DefaultValue", value);
    }

    [Fact]
    public void ValueOrNull_ShouldReturnValue_WhenOptionIsSome()
    {
        var option = Option.Some<string>("TestValue");
        var value = option.ValueOrNull();

        Assert.Equal("TestValue", value);
    }

    [Fact]
    public void ValueOrNull_ShouldReturnNull_WhenOptionIsNone()
    {
        var option = Option.None<string>();
        var value = option.ValueOrNull();

        Assert.Null(value);
    }

    [Fact]
    public void ImplicitOperator_FromNonNullValue_ShouldCreateSomeOption()
    {
        var value = "TestValue";
        Option<string> option = value;

        Assert.True(option.IsSome);
        Assert.Equal("TestValue", option.ValueOrThrow());
    }

    [Fact]
    public void ImplicitOperator_FromNullValue_ShouldCreateNoneOption()
    {
        string? value = null;
        Option<string> option = value;

        Assert.True(option.IsNone);
        Assert.Throws<InvalidOperationException>(() => option.ValueOrThrow());
    }

    [Fact]
    public void ExplicitOperator_FromSomeOption_ShouldReturnNonNullValue()
    {
        var option = Option.Some<string>("TestValue");
        var value = (string?)option;

        Assert.Equal("TestValue", value);
    }

    [Fact]
    public void ExplicitOperator_FromNoneOption_ShouldReturnNullValue()
    {
        var option = Option.None<string>();
        var value = (string?)option;

        Assert.Null(value);
    }
}