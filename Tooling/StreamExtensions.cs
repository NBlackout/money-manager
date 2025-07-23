using System.Text;

namespace Tooling;

public static class StreamExtensions
{
    public static string ToUtf8String(this Stream content)
    {
        using MemoryStream ms = new();
        content.CopyTo(ms);
        content.Position = 0;

        return Encoding.UTF8.GetString(ms.ToArray());
    }
}