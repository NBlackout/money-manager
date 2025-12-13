namespace App.Read.Ports;

public interface IExpectedTransactionSummariesDataSource
{
    Task<ExpectedTransactionSummaryPresentation[]> By(int year, int month);
}

public record ExpectedTransactionSummaryPresentation(DateOnly Date, decimal Amount, string Label, string? CategoryLabel);