using Tooling;

namespace Infra.Shared;

public class CsvHelper : ICsvHelper
{
    private const string CellSeparator = ";";
    private static readonly string LineSeparator = Environment.NewLine;

    public async Task<string[][]> Read(Stream content)
    {
        using StreamReader reader = new(content);
        string? header = await reader.ReadLineAsync();
        if (header == null)
            return [];

        List<string[]> lines = [];
        while (await reader.ReadLineAsync() is { } line)
            lines.Add(Parse(line));

        return [..lines];
    }

    public Task<Stream> Write(string[] headers, string[][] lines)
    {
        string[] rows = [Row(headers), ..lines.Select(Row)];
        string content = string.Join(LineSeparator, rows);

        return Task.FromResult(content.ToUtf8Stream());
    }

    private static string[] Parse(string line) =>
        line.Split(CellSeparator);

    private static string Row(string[] arg) =>
        string.Join(CellSeparator, arg);
}