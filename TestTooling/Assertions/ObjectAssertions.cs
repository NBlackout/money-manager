using System.Text.Json;
using Xunit.Sdk;

namespace TestTooling.Assertions;

public record ObjectAssertions<T>(T? Actual)
{
    public void Be(T? expected) =>
        Assert.Equal(expected, this.Actual);

    public void BeEquivalentTo(T? expected)
    {
        try
        {
            Assert.Equivalent(expected, this.Actual, true);
        }
        catch (XunitException e)
        {
            if (!e.Message.Contains("[]"))
                throw;

            string expectedJson = JsonSerializer.Serialize(expected, Defaults.JsonSerializerOptions);
            string actualJson = JsonSerializer.Serialize(this.Actual, Defaults.JsonSerializerOptions);

            try
            {
                Assert.Equal(expectedJson, actualJson);
            }
            catch (EqualException)
            {
                string expectedPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                File.WriteAllText(expectedPath, expectedJson);

                string actualPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                File.WriteAllText(actualPath, actualJson);

                throw new XunitException(
                    "Actual differs from expected"
                    + Environment.NewLine
                    + $"Compare(Rider): \"file:///{actualPath.Replace("\\", "/")}\",\"file:///{expectedPath.Replace("\\", "/")}\""
                );
            }
        }
    }
}