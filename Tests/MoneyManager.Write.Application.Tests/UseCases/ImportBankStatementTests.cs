using MoneyManager.Write.Infrastructure.OfxProcessing;
using static MoneyManager.Write.Application.Tests.UseCases.ImportBankStatementTests.Data;

namespace MoneyManager.Write.Application.Tests.UseCases;

public class ImportBankStatementTests
{
    private readonly InMemoryAccountRepository repository;
    private readonly StubbedOfxParser ofxParser;
    private readonly ImportBankStatement sut;

    public ImportBankStatementTests()
    {
        this.repository = new InMemoryAccountRepository();
        this.ofxParser = new StubbedOfxParser();
        this.sut = new ImportBankStatement(this.repository, this.ofxParser);
    }

    [Fact]
    public async Task Should_track_unknown_account_assigning_a_default_label()
    {
        AccountBuilder expected = AccountBuilder.For(Guid.NewGuid()) with
        {
            Number = "Number", Label = "Number", Tracked = true
        };
        this.ofxParser.SetAccountStatementFor(TheStream, AccountStatementFrom(expected));
        this.repository.NextId = () => expected.Id;

        await this.Verify_ImportTransactions(TheStream, expected);
    }

    [Fact]
    public async Task Should_synchronize_already_tracked_account()
    {
        AccountBuilder account = AccountBuilder.For(Guid.NewGuid()) with { Balance = 100.00m, Tracked = true };
        this.FeedByExternalId(account);

        const decimal newBalance = 1337.42m;
        this.ofxParser.SetAccountStatementFor(TheStream, AccountStatementFrom(account) with { Balance = newBalance });

        await this.Verify_ImportTransactions(TheStream, account with { Balance = newBalance });
    }

    [Fact]
    public async Task Should_skip_account_no_longer_tracked()
    {
        AccountBuilder account = AccountBuilder.For(Guid.NewGuid()) with { Tracked = false };
        this.FeedByExternalId(account);
        this.ofxParser.SetAccountStatementFor(TheStream, AccountStatementFrom(account));

        await this.Verify_ImportTransactions(TheStream, account);
    }

    private async Task Verify_ImportTransactions(Stream stream, AccountBuilder expected)
    {
        await this.sut.Execute(stream);

        Account actual = await this.repository.GetById(expected.Id);
        actual.Snapshot.Should().Be(expected.Build().Snapshot);
    }

    private void FeedByExternalId(AccountBuilder account) =>
        this.repository.FeedByExternalId(new ExternalId(account.BankIdentifier, account.Number), account.Build());

    private static AccountStatement AccountStatementFrom(AccountBuilder expected) =>
        new(expected.BankIdentifier, expected.Number, expected.Balance);

    internal static class Data
    {
        public static readonly MemoryStream TheStream = new(new byte[] { 0xF0, 0x42 });
    }
}