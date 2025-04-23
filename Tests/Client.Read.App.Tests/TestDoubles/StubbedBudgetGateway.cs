namespace Client.Read.App.Tests.TestDoubles;

public class StubbedBudgetGateway : IBudgetGateway
{
    private BudgetSummaryPresentation[] summaries = null!;

    public Task<BudgetSummaryPresentation[]> Summaries() =>
        Task.FromResult(this.summaries);

    public void Feed(params BudgetSummaryPresentation[] summaries) =>
        this.summaries = summaries;
}