namespace Client.Read.App.UseCases;

public class BudgetSummaries(IBudgetGateway gateway)
{
    public async Task<BudgetSummaryPresentation[]> Execute() =>
        await gateway.Summaries();
}