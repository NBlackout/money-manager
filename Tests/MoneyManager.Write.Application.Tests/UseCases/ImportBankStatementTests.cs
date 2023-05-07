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
        BankBuilder bank = BankBuilder.For(Guid.NewGuid()) with
        {
            ExternalId = "1234567890", Name = "1234567890"
        };
        AccountBuilder account = AccountBuilder.For(Guid.NewGuid()) with
        {
            BankId = bank.Id, Number = "Number", Label = "Number", Tracked = true
        };
        TransactionBuilder aTransaction = TransactionBuilder.For(Guid.NewGuid()) with
        {
            AccountId = account.Id, ExternalId = "V0ZNTODBR8", Amount = 1011.05m
        };
        TransactionBuilder anotherTransaction = TransactionBuilder.For(Guid.NewGuid()) with
        {
            AccountId = account.Id, ExternalId = "193VN20D", Amount = 750.23m
        };

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
        TransactionBuilder existingTransaction = TransactionBuilder.For(Guid.NewGuid()) with
        {
            AccountId = account.Id, ExternalId = "0421NFE9"
        };
        TransactionBuilder unknownTransaction = TransactionBuilder.For(Guid.NewGuid()) with
        {
            AccountId = account.Id, ExternalId = "V02BF05934VE"
        };

        this.FeedByExternalId(bank);
        this.accountRepository.NextId = () => account.Id;
        this.transactionRepository.Feed(existingTransaction.Build());
        this.nextIds = new[] { unknownTransaction.Id };
        this.transactionRepository.NextId = () => this.nextIds[this.nextIdIndex++];
        this.ofxParser.SetAccountStatementFor(TheStream,
            AccountStatementFrom(bank, account, existingTransaction, unknownTransaction));

        await this.Verify_ImportTransactions(TheStream, bank, account, existingTransaction, unknownTransaction);
    }

    [Fact]
    public async Task Should_synchronize_already_tracked_account()
    {
        BankBuilder bank = BankBuilder.For(Guid.NewGuid());
        AccountBuilder account = AccountBuilder.For(Guid.NewGuid()) with
        {
            BankId = bank.Id, Balance = 100.00m, BalanceDate = DateTime.Today.AddDays(-12), Tracked = true
        };
        AccountBuilder expected = account with { Balance = 1337.42m, BalanceDate = DateTime.Today };

        this.FeedByExternalId(bank);
        this.FeedByExternalId(account);
        this.ofxParser.SetAccountStatementFor(TheStream, AccountStatementFrom(bank, expected));

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

    private static AccountStatement AccountStatementFrom(BankBuilder bank, AccountBuilder account,
        params TransactionBuilder[] transactions)
    {
        return new AccountStatement(bank.ExternalId, account.Number, account.Balance, account.BalanceDate,
            transactions.Select(t => new TransactionStatement(t.ExternalId, t.Amount, t.Label)).ToArray());
    }

    internal static class Data
    {
        public static readonly MemoryStream TheStream = new(new byte[] { 0xF0, 0x42 });
    }
}