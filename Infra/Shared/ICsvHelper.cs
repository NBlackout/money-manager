namespace Infra.Shared;

public interface ICsvHelper
{
    Task<string[][]> Read(Stream content);
    Task<Stream> Write(string[] headers, string[][] lines);
}