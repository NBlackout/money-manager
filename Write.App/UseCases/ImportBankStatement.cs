namespace Write.App.UseCases;

public class ImportBankStatement(
    IBankRepository bankRepository,
    IAccountRepository accountRepository,
    ICategoryRepository categoryRepository,
    ITransactionRepository transactionRepository,
    IBankStatementParser bankStatementParser)
{
    public async Task Execute(string fileName, Stream stream)
    {
        AccountStatement statement = await bankStatementParser.Extract(fileName, stream);

        Bank bank = await this.EnsureBankExists(statement);
        Account account = await this.EnsureAccountExists(bank, statement);
        account.Synchronize(statement.Balance, statement.BalanceDate);
        Transaction[] transactions = await this.NewTransactions(account, statement);

        await this.Save(bank, account, transactions);
    }

    private async Task<Bank> EnsureBankExists(AccountStatement statement)
    {
        Bank? bank = await bankRepository.ByExternalIdOrDefault(statement.BankIdentifier);
        if (bank != null)
            return bank;

        Guid id = await bankRepository.NextIdentity();

        return statement.TrackDescribedBank(id);
    }

    private async Task<Account> EnsureAccountExists(Bank bank, AccountStatement statement)
    {
        ExternalId externalId = new(bank.Id, statement.AccountNumber);
        Account? account = await accountRepository.ByExternalIdOrDefault(externalId);
        if (account != null)
            return account;

        Guid id = await accountRepository.NextIdentity();

        return bank.TrackAccount(id, statement.AccountNumber, statement.Balance, statement.BalanceDate);
    }

    private async Task<Transaction[]> NewTransactions(Account account, AccountStatement statement)
    {
        TransactionStatement[] newTransactionStatements = await this.NewTransactionStatements(statement);

        List<Transaction> newTransactions = [];
        foreach (TransactionStatement newTransactionStatement in newTransactionStatements)
        {
            Guid id = await transactionRepository.NextIdentity();
            Category? category = await this.CategoryFrom(newTransactionStatement);

            newTransactions.Add(account.AttachTransaction(id, newTransactionStatement.TransactionIdentifier,
                newTransactionStatement.Amount, newTransactionStatement.Label, newTransactionStatement.Date, category));
        }

        return newTransactions.ToArray();
    }

    private async Task<TransactionStatement[]> NewTransactionStatements(AccountStatement statement)
    {
        string[] unknownExternalIds = await transactionRepository.UnknownExternalIds(
            statement.Transactions.Select(t => t.TransactionIdentifier).ToArray()
        );

        return statement.Transactions.Where(t => unknownExternalIds.Contains(t.TransactionIdentifier)).ToArray();
    }

    private async Task<Category?> CategoryFrom(TransactionStatement statement)
    {
        if (statement.Category == null)
            return null;

        return await categoryRepository.By(statement.Category);
    }

    private async Task Save(Bank bank, Account account, Transaction[] transactions)
    {
        await bankRepository.Save(bank);
        await accountRepository.Save(account);
        foreach (Transaction transaction in transactions)
            await transactionRepository.Save(transaction);
    }
}
