using MoneyManager.Write.Infrastructure.OfxProcessing;
using static MoneyManager.Write.Application.Tests.UseCases.ImportBankStatementTests.Data;

namespace MoneyManager.Write.Application.Tests.UseCases;

public class ImportBankStatementTests
{
    private readonly InMemoryBankRepository bankRepository;
    private readonly InMemoryAccountRepository accountRepository;
    private readonly StubbedOfxParser ofxParser;
    private readonly ImportBankStatement sut;

    public ImportBankStatementTests()
    {
        this.bankRepository = new InMemoryBankRepository();
        this.accountRepository = new InMemoryAccountRepository();
        this.ofxParser = new StubbedOfxParser();
        this.sut = new ImportBankStatement(this.bankRepository, this.accountRepository, this.ofxParser);
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
        this.ofxParser.SetAccountStatementFor(TheStream, AccountStatementFrom(bank, account));

        this.bankRepository.FeedByExternalId(bank.ExternalId, null);
        this.bankRepository.NextId = () => bank.Id;
        this.accountRepository.FeedByExternalId(new ExternalId(account.BankId, account.Number), null);
        this.accountRepository.NextId = () => account.Id;

        await this.Verify_ImportTransactions(TheStream, bank, account);
    }

    [Fact]
    public async Task Should_track_unknown_account()
    {
        BankBuilder bank = BankBuilder.For(Guid.NewGuid());
        this.FeedByExternalId(bank);
        AccountBuilder account = AccountBuilder.For(Guid.NewGuid()) with
        {
            BankId = bank.Id, Number = "Number", Label = "Number", Tracked = true
        };
        this.ofxParser.SetAccountStatementFor(TheStream, AccountStatementFrom(bank, account));

        this.accountRepository.FeedByExternalId(new ExternalId(account.BankId, account.Number), null);
        this.accountRepository.NextId = () => account.Id;

        await this.Verify_ImportTransactions(TheStream, bank, account);
    }

    [Fact]
    public async Task Should_synchronize_already_tracked_account()
    {
        BankBuilder bank = BankBuilder.For(Guid.NewGuid());
        this.FeedByExternalId(bank);
        AccountBuilder account = AccountBuilder.For(Guid.NewGuid()) with
        {
            BankId = bank.Id, Balance = 100.00m, Tracked = true
        };
        this.FeedByExternalId(account);

        const decimal newBalance = 1337.42m;
        this.ofxParser.SetAccountStatementFor(TheStream,
            AccountStatementFrom(bank, account) with { Balance = newBalance });

        await this.Verify_ImportTransactions(TheStream, bank, account with { Balance = newBalance });
    }

    [Fact]
    public async Task Should_skip_no_longer_tracked_account()
    {
        BankBuilder bank = BankBuilder.For(Guid.NewGuid());
        this.FeedByExternalId(bank);
        AccountBuilder account = AccountBuilder.For(Guid.NewGuid()) with { BankId = bank.Id, Tracked = false };
        this.FeedByExternalId(account);
        this.ofxParser.SetAccountStatementFor(TheStream, AccountStatementFrom(bank, account));

        await this.Verify_ImportTransactions(TheStream, bank, account);
    }

    private async Task Verify_ImportTransactions(Stream stream, BankBuilder expectedBank,
        AccountBuilder expectedAccount)
    {
        await this.sut.Execute(stream);

        Bank actualBank = await this.bankRepository.GetById(expectedBank.Id);
        actualBank.Snapshot.Should().Be(expectedBank.Build().Snapshot);

        Account actualAccount = await this.accountRepository.GetById(expectedAccount.Id);
        actualAccount.Snapshot.Should().Be(expectedAccount.Build().Snapshot);
    }

    private void FeedByExternalId(BankBuilder bank) =>
        this.bankRepository.FeedByExternalId(bank.ExternalId, bank.Build());

    private void FeedByExternalId(AccountBuilder account) =>
        this.accountRepository.FeedByExternalId(new ExternalId(account.BankId, account.Number), account.Build());

    private static AccountStatement AccountStatementFrom(BankBuilder bank, AccountBuilder account) =>
        new(bank.ExternalId, account.Number, account.Balance);

    internal static class Data
    {
        public static readonly MemoryStream TheStream = new(new byte[] { 0xF0, 0x42 });
    }
}