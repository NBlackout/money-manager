namespace Read.App.Ports;

public interface ICategorySummariesDataSource
{
    Task<CategorySummaryPresentation[]> Get();
}