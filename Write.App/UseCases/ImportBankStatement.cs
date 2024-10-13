namespace Write.App.UseCases;

public class ImportBankStatement(
    IAccountRepository accountRepository,
    ICategoryRepository categoryRepository,
    ITransactionRepository transactionRepository,
    IBankStatementParser bankStatementParser)
{
    public async Task Execute(string fileName, Stream stream)
    {
        AccountStatement statement = await bankStatementParser.Extract(fileName, stream);

        Account account = await this.EnsureAccountExists(statement);
        account.Synchronize(statement.Balance, statement.BalanceDate);
        (Category[] categories, Transaction[] transactions) = await this.NewTransactions(account, statement);

        await this.Save(account, categories, transactions);
    }

    private async Task<Account> EnsureAccountExists(AccountStatement statement)
    {
        Account? account = await accountRepository.ByOrDefault(statement.AccountNumber);
        if (account != null)
            return account;

        Guid id = await accountRepository.NextIdentity();

        return Account.StartTracking(id, statement.AccountNumber, statement.Balance, statement.BalanceDate);
    }

    private async Task<(Category[], Transaction[])> NewTransactions(Account account, AccountStatement statement)
    {
        TransactionStatement[] newTransactionStatements = await this.NewTransactionStatements(statement);

        Dictionary<string, Category> newCategories = [];
        List<Transaction> newTransactions = [];
        foreach (TransactionStatement newTransactionStatement in newTransactionStatements)
        {
            Guid id = await transactionRepository.NextIdentity();
            if (newTransactionStatement.Category == null)
            {
                newTransactions.Add(account.AttachTransaction(id, newTransactionStatement.Identifier,
                    newTransactionStatement.Amount, newTransactionStatement.Label, newTransactionStatement.Date, null));

                continue;
            }

            if (newCategories.TryGetValue(newTransactionStatement.Category, out Category? newCategory))
            {
                newTransactions.Add(account.AttachTransaction(id, newTransactionStatement.Identifier,
                    newTransactionStatement.Amount, newTransactionStatement.Label, newTransactionStatement.Date,
                    newCategory));

                continue;
            }

            Category? category = await categoryRepository.ByOrDefault(newTransactionStatement.Category);
            if (category == null)
            {
                category = new Category(await categoryRepository.NextIdentity(), newTransactionStatement.Category!,
                    newTransactionStatement.Category!);

                newCategories.Add(newTransactionStatement.Category, category);
            }

            newTransactions.Add(account.AttachTransaction(id, newTransactionStatement.Identifier,
                newTransactionStatement.Amount, newTransactionStatement.Label, newTransactionStatement.Date, category));
        }

        return (newCategories.Values.ToArray(), newTransactions.ToArray());
    }

    private async Task<TransactionStatement[]> NewTransactionStatements(AccountStatement statement)
    {
        string[] unknownExternalIds = await transactionRepository.UnknownExternalIds(
            statement.Transactions.Select(t => t.Identifier).ToArray()
        );

        return statement.Transactions.Where(t => unknownExternalIds.Contains(t.Identifier)).ToArray();
    }

    private async Task Save(Account account, Category[] categories, Transaction[] transactions)
    {
        await accountRepository.Save(account);
        foreach (Category category in categories)
            await categoryRepository.Save(category);
        foreach (Transaction transaction in transactions)
            await transactionRepository.Save(transaction);
    }
}
