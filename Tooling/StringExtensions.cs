using System.Text;

namespace Tooling;

public static class StringExtensions
{
    public static Stream ToUtf8Stream(this string value) =>
        new MemoryStream(Encoding.UTF8.GetBytes(value));
}