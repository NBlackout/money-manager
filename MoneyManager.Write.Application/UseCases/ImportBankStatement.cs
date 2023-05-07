namespace MoneyManager.Write.Application.UseCases;

public class ImportBankStatement
{
    private readonly IBankRepository bankRepository;
    private readonly IAccountRepository accountRepository;
    private readonly ITransactionRepository transactionRepository;
    private readonly IOfxParser ofxParser;

    public ImportBankStatement(IBankRepository bankRepository, IAccountRepository accountRepository,
        ITransactionRepository transactionRepository, IOfxParser ofxParser)
    {
        this.bankRepository = bankRepository;
        this.accountRepository = accountRepository;
        this.transactionRepository = transactionRepository;
        this.ofxParser = ofxParser;
    }

    public async Task Execute(Stream stream)
    {
        AccountStatement statement = await this.ofxParser.ExtractAccountStatement(stream);

        Bank bank = await this.EnsureBankExists(statement);
        Account account = await this.EnsureAccountIsTracked(bank, statement);
        account.Synchronize(statement.Balance, statement.BalanceDate);
        Transaction[] transactions = await this.UnknownTransactions(account, statement);

        await this.Save(bank, account, transactions);
    }

    private async Task<Bank> EnsureBankExists(AccountStatement statement)
    {
        Bank? bank = await this.bankRepository.ByExternalIdOrDefault(statement.BankIdentifier);
        if (bank != null)
            return bank;

        Guid id = await this.bankRepository.NextIdentity();

        return statement.TrackDescribedBank(id);
    }

    private async Task<Account> EnsureAccountIsTracked(Bank bank, AccountStatement statement)
    {
        ExternalId externalId = new(bank.Id, statement.AccountNumber);
        Account? account = await this.accountRepository.ByExternalIdOrDefault(externalId);
        if (account != null)
            return account;

        Guid id = await this.accountRepository.NextIdentity();

        return bank.TrackAccount(id, statement.AccountNumber, statement.Balance, statement.BalanceDate);
    }

    private async Task<Transaction[]> UnknownTransactions(Account account, AccountStatement statement)
    {
        Dictionary<string, TransactionStatement> transactionStatements =
            statement.Transactions.ToDictionary(t => t.TransactionIdentifier);
        IReadOnlyCollection<string> unknownExternalIds =
            await this.transactionRepository.UnknownExternalIds(transactionStatements.Keys);

        List<Task<Transaction>> unknownTransactionTasks = unknownExternalIds.Select(unknownExternalId =>
            this.UnknownTransaction(account, transactionStatements[unknownExternalId])).ToList();
        await Task.WhenAll(unknownTransactionTasks);

        return unknownTransactionTasks.Select(task => task.Result).ToArray();
    }

    private async Task<Transaction> UnknownTransaction(Account account, TransactionStatement statement)
    {
        Guid id = await this.transactionRepository.NextIdentity();

        return account.AttachTransaction(id, statement.TransactionIdentifier, statement.Amount, statement.Label);
    }

    private async Task Save(Bank bank, Account account, IEnumerable<Transaction> transactions)
    {
        await this.bankRepository.Save(bank);
        await this.accountRepository.Save(account);
        foreach (Transaction transaction in transactions)
            await this.transactionRepository.Save(transaction);
    }
}