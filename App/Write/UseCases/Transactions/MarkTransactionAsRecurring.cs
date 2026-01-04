using App.Write.Model.RecurringTransactions;
using App.Write.Model.Transactions;
using App.Write.Ports;

namespace App.Write.UseCases.Transactions;

public class MarkTransactionAsRecurring(ITransactionRepository transactionRepository, IRecurringTransactionRepository recurringTransactionRepository)
{
    public async Task Execute(TransactionId id, RecurringTransactionId recurringTransactionId)
    {
        Transaction transaction = await transactionRepository.By(id);
        RecurringTransaction recurringTransaction = transaction.MarkAsRecurring(recurringTransactionId);
        await this.Save(transaction, recurringTransaction);
    }

    private async Task Save(Transaction transaction, RecurringTransaction recurringTransaction)
    {
        await transactionRepository.Save(transaction);
        await recurringTransactionRepository.Save(recurringTransaction);
    }
}