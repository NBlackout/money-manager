namespace MoneyManager.Client.Read.Application.Ports;

public interface ICategoryGateway
{
    Task<IReadOnlyCollection<CategorySummaryPresentation>> Summaries();
}