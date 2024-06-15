namespace Read.App.Ports;

public interface ICategorySummariesDataSource
{
    Task<IReadOnlyCollection<CategorySummaryPresentation>> Get();
}