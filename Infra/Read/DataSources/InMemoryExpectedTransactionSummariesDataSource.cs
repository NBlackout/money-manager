using App.Read.Ports;
using App.Write.Model.RecurringTransactions;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryExpectedTransactionSummariesDataSource(
    InMemoryRecurringTransactionRepository recurringTransactionRepository,
    InMemoryCategoryRepository categoryRepository
) : IExpectedTransactionSummariesDataSource
{
    public Task<ExpectedTransactionSummaryPresentation[]> By(int year, int month)
    {
        ExpectedTransactionSummaryPresentation[] recurringTransactions = recurringTransactionRepository
            .Data
            .Where(t => t.Date.Year == year && t.Date.Month == month)
            .Select(this.ToPresentation)
            .ToArray();

        return Task.FromResult(recurringTransactions);
    }

    private ExpectedTransactionSummaryPresentation ToPresentation(RecurringTransactionSnapshot recurringTransaction)
    {
        string? categoryLabel = recurringTransaction.CategoryId is not null
            ? categoryRepository.Data.Single(c => c.Id == recurringTransaction.CategoryId).Label
            : null;

        return new ExpectedTransactionSummaryPresentation(recurringTransaction.Date, recurringTransaction.Amount, recurringTransaction.Label, categoryLabel);
    }
}