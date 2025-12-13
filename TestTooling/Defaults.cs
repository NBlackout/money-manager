using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestTooling;

public static class Defaults
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true, Converters = { new JsonStringEnumConverter() }, PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}