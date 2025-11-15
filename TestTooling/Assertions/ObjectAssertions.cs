using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit.Sdk;

namespace TestTooling.Assertions;

public record ObjectAssertions<T>(T? Actual)
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true, Converters = { new JsonStringEnumConverter(), new JsonDecimalConverter() }
    };

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

            string expectedJson = JsonSerializer.Serialize(expected, this.jsonSerializerOptions);
            string actualJson = JsonSerializer.Serialize(this.Actual, this.jsonSerializerOptions);

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
                    "Actual differs from expected" +
                    Environment.NewLine +
                    $"Compare(Rider): \"file:///{actualPath.Replace("\\", "/")}\",\"file:///{expectedPath.Replace("\\", "/")}\""
                );
            }
        }
    }

    private sealed class JsonDecimalConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            reader.GetDecimal();

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options) =>
            writer.WriteRawValue(value.ToString("F", CultureInfo.InvariantCulture));
    }
}