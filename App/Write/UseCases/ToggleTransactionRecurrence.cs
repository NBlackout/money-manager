using App.Write.Model.Transactions;
using App.Write.Ports;

namespace App.Write.UseCases;

public class ToggleTransactionRecurrence(ITransactionRepository repository)
{
    public async Task Execute(TransactionId id)
    {
        Transaction transaction = await repository.By(id);
        transaction.ToggleRecurrence();
        await repository.Save(transaction);
    }
}