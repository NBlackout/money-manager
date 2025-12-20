using App.Write.Model.Accounts;
using App.Write.Model.Categories;

namespace App.Write.Model.Transactions;

public record TransactionSnapshot(
    TransactionId Id,
    AccountId AccountId,
    string ExternalId,
    decimal Amount,
    string Label,
    DateOnly Date,
    CategoryId? CategoryId,
    bool IsRecurring
) : ISnapshot<TransactionId>;
