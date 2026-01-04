using App.Write.Model.Transactions;
using App.Write.Model.ValueObjects;
using App.Write.Ports;

namespace App.Write.UseCases.Transactions;

public class PreferTransactionLabel(ITransactionRepository transactionRepository)
{
    public async Task Execute(TransactionId id, Label label)
    {
        Transaction transaction = await transactionRepository.By(id);
        transaction.Prefer(label);
        await transactionRepository.Save(transaction);
    }
}