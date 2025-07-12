using Xunit.Sdk;

namespace Shared.TestTooling.Assertions;

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

            ((object?)this.Actual).Should().BeEquivalentTo(expected);
        }
    }
}