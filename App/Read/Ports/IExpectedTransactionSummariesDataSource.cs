namespace App.Read.Ports;

public interface IExpectedTransactionSummariesDataSource
{
    Task<ExpectedTransactionSummaryPresentation[]> All();
}

public record ExpectedTransactionSummaryPresentation(decimal Amount, string Label);