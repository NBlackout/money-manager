using App.Write.Ports;
using Tooling;

namespace App.Tests.Write.TestDoubles;

public class StubbedCategoryImporter : ICategoryImporter
{
    private readonly Dictionary<string, CategoryToImport[]> data = [];

    public Task<CategoryToImport[]> Parse(Stream content) =>
        Task.FromResult(this.data[content.ToUtf8String()]);

    public void Feed(Stream content, CategoryToImport[] categoryToImport) =>
        this.data[content.ToUtf8String()] = categoryToImport;
}