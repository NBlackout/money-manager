namespace App.Read.Ports;

public interface ICategorizationRuleSummariesDataSource
{
    Task<CategorizationRuleSummaryPresentation[]> All();
}

public record CategorizationRuleSummaryPresentation(Guid Id, Guid CategoryId, string CategoryLabel, string Keywords, decimal? Amount, decimal? Margin);