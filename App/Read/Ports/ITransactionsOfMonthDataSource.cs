namespace App.Read.Ports;

public interface ITransactionsOfMonthDataSource
{
    Task<TransactionSummaryPresentation[]> By(Guid accountId, int year, int month);
}

public record TransactionSummaryPresentation(Guid Id, decimal Amount, string Label, DateOnly Date, string? Category);