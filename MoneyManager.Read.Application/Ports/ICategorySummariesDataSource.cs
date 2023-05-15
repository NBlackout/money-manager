namespace MoneyManager.Read.Application.Ports;

public interface ICategorySummariesDataSource
{
    Task<IReadOnlyCollection<CategorySummaryPresentation>> Get();
}