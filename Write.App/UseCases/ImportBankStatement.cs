namespace Write.App.UseCases;

public class ImportBankStatement(
    IBankRepository bankRepository,
    IAccountRepository accountRepository,
    ITransactionRepository transactionRepository,
    IBankStatementParser bankStatementParser)
{
    public async Task Execute(string fileName, Stream stream)
    {
        AccountStatement statement = await bankStatementParser.ExtractAccountStatement(fileName, stream);

        Bank bank = await this.EnsureBankExists(statement);
        Account account = await this.EnsureAccountIsTracked(bank, statement);
        account.Synchronize(statement.Balance, statement.BalanceDate);
        Transaction[] transactions = await this.UnknownTransactions(account, statement);

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

    private async Task<Account> EnsureAccountIsTracked(Bank bank, AccountStatement statement)
    {
        ExternalId externalId = new(bank.Id, statement.AccountNumber);
        Account? account = await accountRepository.ByExternalIdOrDefault(externalId);
        if (account != null)
            return account;

        Guid id = await accountRepository.NextIdentity();

        return bank.TrackAccount(id, statement.AccountNumber, statement.Balance, statement.BalanceDate);
    }

    private async Task<Transaction[]> UnknownTransactions(Account account, AccountStatement statement)
    {
        Dictionary<string, TransactionStatement> transactionStatements =
            statement.Transactions.ToDictionary(t => t.TransactionIdentifier);
        string[] unknownExternalIds =
            await transactionRepository.UnknownExternalIds(transactionStatements.Keys);

        List<Task<Transaction>> unknownTransactionTasks = unknownExternalIds.Select(unknownExternalId =>
            this.UnknownTransaction(account, transactionStatements[unknownExternalId])).ToList();
        
        // Stryker disable all: TODO find a way to cover this instruction. Because we return Task.FromResult in test doubles, tasks are already completed...
        await Task.WhenAll(unknownTransactionTasks);

        return unknownTransactionTasks.Select(task => task.Result).ToArray();
    }

    private async Task<Transaction> UnknownTransaction(Account account, TransactionStatement statement)
    {
        Guid id = await transactionRepository.NextIdentity();

        return account.AttachTransaction(id, statement.TransactionIdentifier, statement.Amount, statement.Label, statement.Date);
    }

    private async Task Save(Bank bank, Account account, IEnumerable<Transaction> transactions)
    {
        await bankRepository.Save(bank);
        await accountRepository.Save(account);
        foreach (Transaction transaction in transactions)
            await transactionRepository.Save(transaction);
    }
}