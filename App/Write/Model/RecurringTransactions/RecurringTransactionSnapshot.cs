using App.Write.Model.Categories;

namespace App.Write.Model.RecurringTransactions;

public record RecurringTransactionSnapshot(RecurringTransactionId Id, decimal Amount, string Label, DateOnly Date, CategoryId? CategoryId)
    : ISnapshot<RecurringTransactionId>;