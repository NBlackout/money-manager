namespace Client.Read.Infra.Gateways.Budget;

public class StubbedBudgetGateway : IBudgetGateway
{
    private BudgetSummaryPresentation[] summaries = null!;

    public Task<BudgetSummaryPresentation[]> Summaries() =>
        Task.FromResult(this.summaries);

    public void Feed(params BudgetSummaryPresentation[] expected) =>
        this.summaries = expected;
}