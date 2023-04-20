using MoneyManager.Infrastructure.Write.OfxProcessor;
using static MoneyManager.Application.Write.Tests.ImportTransactionsTests.Data;

namespace MoneyManager.Application.Write.Tests;

public class ImportTransactionsTests
{
    private readonly InMemoryAccountRepository repository;
    private readonly StubbedOfxProcessor ofxProcessor;
    private readonly ImportTransactions sut;

    public ImportTransactionsTests()
    {
        this.repository = new InMemoryAccountRepository();
        this.ofxProcessor = new StubbedOfxProcessor();
        this.sut = new ImportTransactions(this.repository, this.ofxProcessor);
    }

    [Fact]
    public async Task Should_track_unknown_account()
    {
        this.ofxProcessor.SetResultFor(TheStream, TheAccountId);

        await this.sut.Execute(TheStream);

        Account actual = await this.repository.GetById(TheAccountId);
        actual.Should().BeEquivalentTo(new Account(TheAccountId));
    }

    [Fact]
    public async Task Should_skip_already_tracked_account()
    {
        Account existing = new(TheAccountId);
        this.repository.Feed(existing);
        this.ofxProcessor.SetResultFor(TheStream, TheAccountId);

        await this.sut.Execute(TheStream);

        Account actual = await this.repository.GetById(existing.Id);
        actual.Should().BeEquivalentTo(existing);
    }

    internal static class Data
    {
        public static readonly MemoryStream TheStream = new(new byte[] { 0x01, 0x02 });
        public static readonly AccountId TheAccountId = new("BankId", "Account number");
    }
}