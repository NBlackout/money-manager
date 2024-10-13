using System.Text.Json;

namespace Shared.TestTooling;

public static class Defaults
{
    public static readonly JsonSerializerOptions JsonSerializerOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
}