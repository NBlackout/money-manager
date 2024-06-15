namespace Client.Read.App.Ports;

public interface ICategoryGateway
{
    Task<IReadOnlyCollection<CategorySummaryPresentation>> Summaries();
}