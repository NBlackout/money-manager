using Write.Infra.BankStatementParsing;
using static Shared.TestTooling.Randomizer;
using static Write.App.Tests.UseCases.ImportBankStatementTests.Data;

namespace Write.App.Tests.UseCases;

public class ImportBankStatementTests
{
    private readonly InMemoryBankRepository bankRepository = new();
    private readonly InMemoryAccountRepository accountRepository = new();
    private readonly InMemoryTransactionRepository transactionRepository = new();
    private readonly StubbedBankStatementParser bankStatementParser = new();
    private readonly ImportBankStatement sut;

    private Guid[] nextIds = [];
    private int nextIdIndex;

    public ImportBankStatementTests()
    {
        this.sut = new ImportBankStatement(this.bankRepository, this.accountRepository, this.transactionRepository,
            this.bankStatementParser);
    }

    [Fact]
    public async Task Should_track_unknown_bank_and_account()
    {
        BankBuilder bank = BankBuilder.Create();
        AccountBuilder account = AccountBuilder.Create() with
        {
            BankId = bank.Id, Number = "Number", Label = "Number", Tracked = true
        };
        TransactionBuilder aTransaction = ATransaction(Guid.NewGuid(), account.Id);
        TransactionBuilder anotherTransaction = ATransaction(Guid.NewGuid(), account.Id);

        this.bankRepository.NextId = () => bank.Id;
        this.accountRepository.NextId = () => account.Id;
        this.nextIds = [aTransaction.Id, anotherTransaction.Id];
        this.transactionRepository.NextId = () => this.nextIds[this.nextIdIndex++];
        this.bankStatementParser.SetAccountStatementFor(TheStream,
            AccountStatementFrom(bank, account, aTransaction, anotherTransaction));

        await this.Verify(TheStream, bank, account, aTransaction, anotherTransaction);
    }

    [Fact]
    public async Task Should_track_unknown_account()
    {
        BankBuilder bank = BankBuilder.Create();
        AccountBuilder account = AccountBuilder.Create() with
        {
            BankId = bank.Id, Number = "Number", Label = "Number", Tracked = true
        };
        TransactionBuilder aTransaction = ATransaction(Guid.NewGuid(), account.Id);
        TransactionBuilder anotherTransaction = ATransaction(Guid.NewGuid(), account.Id);

        this.FeedByExternalId(bank);
        this.accountRepository.NextId = () => account.Id;
        this.nextIds = [aTransaction.Id, anotherTransaction.Id];
        this.transactionRepository.NextId = () => this.nextIds[this.nextIdIndex++];
        this.bankStatementParser.SetAccountStatementFor(TheStream,
            AccountStatementFrom(bank, account, aTransaction, anotherTransaction));

        await this.Verify(TheStream, bank, account, aTransaction, anotherTransaction);
    }

    [Fact]
    public async Task Should_synchronize_already_tracked_account()
    {
        BankBuilder bank = BankBuilder.Create();
        AccountBuilder existing = AccountBuilder.Create() with
        {
            BankId = bank.Id, Balance = 100.00m, BalanceDate = DateTime.Today.AddDays(-12), Tracked = true
        };
        AccountBuilder expected = existing with { Balance = 1337.42m, BalanceDate = DateTime.Today };

        this.FeedByExternalId(bank);
        this.FeedByExternalId(existing);
        AccountStatement accountStatement = AccountStatementFrom(bank, expected);
        this.bankStatementParser.SetAccountStatementFor(TheStream, accountStatement);

        await this.Verify(TheStream, bank, expected);
    }

    [Fact]
    public async Task Should_skip_no_longer_tracked_account()
    {
        BankBuilder bank = Any<BankBuilder>();
        AccountBuilder account = Any<AccountBuilder>() with { BankId = bank.Id, Tracked = false };
        AccountStatement statement = Any<AccountStatement>() with
        {
            BankIdentifier = bank.ExternalId, AccountNumber = account.Number, Transactions = []
        };

        this.FeedByExternalId(bank);
        this.FeedByExternalId(account);
        this.bankStatementParser.SetAccountStatementFor(TheStream, statement);

        await this.Verify(TheStream, bank, account);
    }

    private async Task Verify(Stream stream, BankBuilder expectedBank,
        AccountBuilder expectedAccount, params TransactionBuilder[] expectedTransactions)
    {
        await this.sut.Execute(stream);

        Bank actualBank = await this.bankRepository.By(expectedBank.Id);
        actualBank.Snapshot.Should().Be(expectedBank.ToSnapshot());

        Account actualAccount = await this.accountRepository.By(expectedAccount.Id);
        actualAccount.Snapshot.Should().Be(expectedAccount.ToSnapshot());

        foreach (TransactionBuilder expectedTransaction in expectedTransactions)
        {
            Transaction actualTransaction = await this.transactionRepository.By(expectedTransaction.Id);
            actualTransaction.Snapshot.Should().Be(expectedTransaction.ToSnapshot());
        }
    }

    private void FeedByExternalId(BankBuilder bank) =>
        this.bankRepository.FeedByExternalId(bank.ExternalId, bank.Build());

    private void FeedByExternalId(AccountBuilder account) =>
        this.accountRepository.FeedByExternalId(new ExternalId(account.BankId, account.Number), account.Build());

    internal static class Data
    {
        public static readonly MemoryStream TheStream = new([0xF0, 0x42]);

        public static TransactionBuilder ATransaction(Guid id, Guid accountId)
        {
            return TransactionBuilder.For(id) with
            {
                AccountId = accountId, ExternalId = id.ToString(), CategoryId = null
            };
        }

        public static AccountStatement AccountStatementFrom(BankBuilder bank, AccountBuilder account,
            params TransactionBuilder[] transactions)
        {
            return new AccountStatement(bank.ExternalId, account.Number, account.Balance, account.BalanceDate,
                transactions.Select(t => new TransactionStatement(t.ExternalId, t.Amount, t.Label, t.Date)).ToArray());
        }
    }
}