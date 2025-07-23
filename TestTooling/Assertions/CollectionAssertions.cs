using Xunit.Sdk;

namespace TestTooling.Assertions;

public record CollectionAssertions<T>(IEnumerable<T> Actual)
{
    public void Equal(params T[] expected)
    {
        try
        {
            Assert.Equal(expected, this.Actual);
        }
        catch (XunitException e)
        {
            if (!e.Message.Contains("[]"))
                throw;

            this.Actual.Should().BeEquivalentTo(expected);
        }
    }
}