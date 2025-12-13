using App.Write.Model.Accounts;
using App.Write.Model.Categories;
using App.Write.Model.RecurringTransactions;
using App.Write.Model.Transactions;

namespace App.Tests.Write.Tooling;

public static class SnapshotHelpers
{
    public static AccountSnapshot AnAccount() =>
        Any<AccountSnapshot>();

    public static CategorySnapshot ACategory() =>
        Any<CategorySnapshot>();

    public static RecurringTransactionSnapshot ARecurringTransaction() =>
        Any<RecurringTransactionSnapshot>();

    public static TransactionSnapshot ATransaction() =>
        Any<TransactionSnapshot>();
}