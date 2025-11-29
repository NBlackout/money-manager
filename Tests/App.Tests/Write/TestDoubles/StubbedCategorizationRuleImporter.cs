using App.Write.Ports;
using Tooling;

namespace App.Tests.Write.TestDoubles;

public class StubbedCategorizationRuleImporter : ICategorizationRuleImporter
{
    private readonly Dictionary<string, CategorizationRuleToImport[]> data = [];

    public Task<CategorizationRuleToImport[]> Parse(Stream content) =>
        Task.FromResult(this.data[content.ToUtf8String()]);

    public void Feed(Stream content, CategorizationRuleToImport[] categorizationRuleToImport) =>
        this.data[content.ToUtf8String()] = categorizationRuleToImport;
}