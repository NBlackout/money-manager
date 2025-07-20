using App.Write.Model.Categories;
using App.Write.Model.Transactions;
using App.Write.Model.ValueObjects;

namespace App.Write.Ports;

public interface ITransactionRepository
{
    Task<TransactionId> NextIdentity();
    Task<Transaction> By(TransactionId id);
    Task<Transaction[]> By(CategoryId id);
    Task<ExternalId[]> UnknownExternalIds(IEnumerable<ExternalId> externalIds);
    Task Save(params Transaction[] transactions);
}