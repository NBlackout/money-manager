namespace App.Read.Ports;

public interface ICategorySummariesDataSource
{
    Task<CategorySummaryPresentation[]> All();
}

public record CategorySummaryPresentation(Guid Id, string Label, params ChildCategorySummaryPresentation[] Children);

public record ChildCategorySummaryPresentation(Guid Id, string Label);