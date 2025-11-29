using App.Read.Ports;

namespace App.Tests.Read.TestDoubles;

public class StubbedCategorizationRuleSummariesDataSource : ICategorizationRuleSummariesDataSource
{
    private CategorizationRuleSummaryPresentation[] data = null!;

    public Task<CategorizationRuleSummaryPresentation[]> All() =>
        Task.FromResult(this.data);

    public void Feed(CategorizationRuleSummaryPresentation[] summaries) =>
        this.data = summaries;
}