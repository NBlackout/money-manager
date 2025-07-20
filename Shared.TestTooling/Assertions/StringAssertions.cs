namespace Shared.TestTooling.Assertions;

public record StringAssertions(string Actual)
{
    public void Be(string? expected) =>
        Assert.Equal(expected, this.Actual);
}