namespace Shared.TestTooling.Assertions;

public record BooleanAssertions(bool? Actual)
{
    public void Be(bool expected) =>
        Assert.Equal(expected, this.Actual);

    public void BeTrue(string because = "") =>
        Assert.True(this.Actual, because);

    public void BeFalse(string because = "") =>
        Assert.False(this.Actual, because);
}