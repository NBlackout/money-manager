namespace App.Read.Ports;

public interface ICategorySummariesDataSource
{
    Task<CategorySummaryPresentation[]> All();
}

public record CategorySummaryPresentation(Guid Id, string Label);