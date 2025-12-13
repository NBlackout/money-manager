using App.Write.Model.RecurringTransactions;

namespace App.Write.Ports;

public interface IRecurringTransactionRepository
{
    Task Save(RecurringTransaction recurringTransaction);
}