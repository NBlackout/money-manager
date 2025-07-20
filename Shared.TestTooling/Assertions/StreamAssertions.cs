using System.Text;

namespace Shared.TestTooling.Assertions;

public record StreamAssertions(Stream Actual)
{
    public void Equal(Stream expected)
    {
        byte[] expectedContent = Read(expected);
        byte[] actualContent = Read(this.Actual);

        actualContent.Should().Equal(expectedContent);
    }

    public void Equal(string expected)
    {
        byte[] actualContent = Read(this.Actual);

        Encoding.UTF8.GetString(actualContent).Should().Be(expected);
    }

    private static byte[] Read(Stream content)
    {
        using MemoryStream buffer = new();
        content.Position = 0;
        content.CopyTo(buffer);

        return buffer.ToArray();
    }
}