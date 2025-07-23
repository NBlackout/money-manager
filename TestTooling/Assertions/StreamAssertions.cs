using Tooling;

namespace TestTooling.Assertions;

public record StreamAssertions(Stream Actual)
{
    public void Equal(Stream expected) =>
        this.Actual.ToUtf8String().Should().Be(expected.ToUtf8String());

    public void Equal(string expected) =>
        this.Actual.ToUtf8String().Should().Be(expected);
}