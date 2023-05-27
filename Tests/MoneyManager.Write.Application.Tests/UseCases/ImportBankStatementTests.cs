using MoneyManager.Write.Infrastructure.OfxProcessing;
using static MoneyManager.Write.Application.Tests.UseCases.ImportBankStatementTests.Data;

namespace MoneyManager.Write.Application.Tests.UseCases;

public class ImportBankStatementTests
{
    private readonly InMemoryBankRepository bankRepository;
    private readonly InMemoryAccountRepository accountRepository;
    private readonly InMemoryTransactionRepository transactionRepository;
    private readonly StubbedOfxParser ofxParser;
    private readonly ImportBankStatement sut;

    private Guid[] nextIds = Array.Empty<Guid>();
    private int nextIdIndex;

    public ImportBankStatementTests()
    {
        this.bankRepository = new InMemoryBankRepository();
        this.accountRepository = new InMemoryAccountRepository();
        this.transactionRepository = new InMemoryTransactionRepository();
        this.ofxParser = new StubbedOfxParser();
        this.sut = new ImportBankStatement(this.bankRepository, this.accountRepository, this.transactionRepository,
            this.ofxParser);
    }

    [Fact]
    public async Task Should_track_unknown_bank_and_account()
    {
        BankBuilder bank = BankBuilder.For(Guid.NewGuid());
        AccountBuilder account = AccountBuilder.For(Guid.NewGuid()) with
        {
            BankId = bank.Id, Number = "Number", Label = "Number", Tracked = true
        };
        TransactionBuilder aTransaction = ATransaction(Guid.NewGuid(), account.Id);
        TransactionBuilder anotherTransaction = ATransaction(Guid.NewGuid(), account.Id);

        this.bankRepository.NextId = () => bank.Id;
        this.accountRepository.NextId = () => account.Id;
        this.nextIds = new[] { aTransaction.Id, anotherTransaction.Id };
        this.transactionRepository.NextId = () => this.nextIds[this.nextIdIndex++];
        this.ofxParser.SetAccountStatementFor(TheStream,
            AccountStatementFrom(bank, account, aTransaction, anotherTransaction));

        await this.Verify_ImportTransactions(TheStream, bank, account, aTransaction, anotherTransaction);
    }

    [Fact]
    public async Task Should_track_unknown_account()
    {
        BankBuilder bank = BankBuilder.For(Guid.NewGuid());
        AccountBuilder account = AccountBuilder.For(Guid.NewGuid()) with
        {
            BankId = bank.Id, Number = "Number", Label = "Number", Tracked = true
        };
        TransactionBuilder aTransaction = ATransaction(Guid.NewGuid(), account.Id);
        TransactionBuilder anotherTransaction = ATransaction(Guid.NewGuid(), account.Id);

        this.FeedByExternalId(bank);
        this.accountRepository.NextId = () => account.Id;
        this.nextIds = new[] { aTransaction.Id, anotherTransaction.Id };
        this.transactionRepository.NextId = () => this.nextIds[this.nextIdIndex++];
        this.ofxParser.SetAccountStatementFor(TheStream,
            AccountStatementFrom(bank, account, aTransaction, anotherTransaction));

        await this.Verify_ImportTransactions(TheStream, bank, account, aTransaction, anotherTransaction);
    }

    [Fact]
    public async Task Should_synchronize_already_tracked_account()
    {
        BankBuilder bank = BankBuilder.For(Guid.NewGuid());
        AccountBuilder existing = AccountBuilder.For(Guid.NewGuid()) with
        {
            BankId = bank.Id, Balance = 100.00m, BalanceDate = DateTime.Today.AddDays(-12), Tracked = true
        };
        AccountBuilder expected = existing with { Balance = 1337.42m, BalanceDate = DateTime.Today };

        this.FeedByExternalId(bank);
        this.FeedByExternalId(existing);
        AccountStatement accountStatement = AccountStatementFrom(bank, expected);
        this.ofxParser.SetAccountStatementFor(TheStream, accountStatement);

        await this.Verify_ImportTransactions(TheStream, bank, expected);
    }

    [Fact]
    public async Task Should_skip_no_longer_tracked_account()
    {
        BankBuilder bank = BankBuilder.For(Guid.NewGuid());
        AccountBuilder account = AccountBuilder.For(Guid.NewGuid()) with { BankId = bank.Id, Tracked = false };

        this.FeedByExternalId(bank);
        this.FeedByExternalId(account);
        this.ofxParser.SetAccountStatementFor(TheStream, AccountStatementFrom(bank, account));

        await this.Verify_ImportTransactions(TheStream, bank, account);
    }

    private async Task Verify_ImportTransactions(Stream stream, BankBuilder expectedBank,
        AccountBuilder expectedAccount, params TransactionBuilder[] expectedTransactions)
    {
        await this.sut.Execute(stream);

        Bank actualBank = await this.bankRepository.ById(expectedBank.Id);
        actualBank.Snapshot.Should().Be(expectedBank.Build().Snapshot);

        Account actualAccount = await this.accountRepository.ById(expectedAccount.Id);
        actualAccount.Snapshot.Should().Be(expectedAccount.Build().Snapshot);

        foreach (TransactionBuilder expectedTransaction in expectedTransactions)
        {
            Transaction actualTransaction = await this.transactionRepository.ById(expectedTransaction.Id);
            actualTransaction.Snapshot.Should().Be(expectedTransaction.Build().Snapshot);
        }
    }

    private void FeedByExternalId(BankBuilder bank) =>
        this.bankRepository.FeedByExternalId(bank.ExternalId, bank.Build());

    private void FeedByExternalId(AccountBuilder account) =>
        this.accountRepository.FeedByExternalId(new ExternalId(account.BankId, account.Number), account.Build());

    internal static class Data
    {
        public static readonly MemoryStream TheStream = new(new byte[] { 0xF0, 0x42 });

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