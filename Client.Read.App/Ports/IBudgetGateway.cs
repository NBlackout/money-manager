namespace Client.Read.App.Ports;

public interface IBudgetGateway
{
    Task<BudgetSummaryPresentation[]> Summaries();
}