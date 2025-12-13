namespace App.Tests.Read.Tooling;

public static class BuilderHelpers
{
    public static AccountBuilder AnAccount() =>
        Any<AccountBuilder>();

    public static BudgetBuilder ABudget() =>
        Any<BudgetBuilder>();

    public static CategoryBuilder ACategory() =>
        Any<CategoryBuilder>();

    public static RecurringTransactionBuilder ARecurringTransaction() =>
        Any<RecurringTransactionBuilder>();

    public static TransactionBuilder ATransaction() =>
        Any<TransactionBuilder>();
}