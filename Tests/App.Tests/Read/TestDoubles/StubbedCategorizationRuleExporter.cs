using System.Text.Json;
using App.Read.Ports;

namespace App.Tests.Read.TestDoubles;

public class StubbedCategorizationRuleExporter : ICategorizationRuleExporter
{
    private readonly Dictionary<string, Stream> data = [];

    public Task<Stream> Export(CategorizationRuleSummaryPresentation[] categories) =>
        Task.FromResult(this.data[Serialize(categories)]);

    public void Feed(CategorizationRuleSummaryPresentation[] categories, Stream content) =>
        this.data[Serialize(categories)] = content;

    private static string Serialize(CategorizationRuleSummaryPresentation[] categories) =>
        JsonSerializer.Serialize(categories);
}