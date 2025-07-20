using System.Text.Json;
using App.Read.Ports;

namespace App.Tests.Read.TestDoubles;

public class StubbedCategoryExporter : ICategoryExporter
{
    private readonly Dictionary<string, Stream> data = [];

    public Task<Stream> Export(CategorySummaryPresentation[] categories) =>
        Task.FromResult(this.data[Serialize(categories)]);

    public void Feed(CategorySummaryPresentation[] categories, Stream content) =>
        this.data[Serialize(categories)] = content;

    private static string Serialize(CategorySummaryPresentation[] categories) =>
        JsonSerializer.Serialize(categories);
}