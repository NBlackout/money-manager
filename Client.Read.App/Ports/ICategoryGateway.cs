namespace Client.Read.App.Ports;

public interface ICategoryGateway
{
    Task<CategorySummaryPresentation[]> Summaries();
}