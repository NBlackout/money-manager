using MoneyManager.Application.Write.Ports;
using MoneyManager.Infrastructure.Write.OfxProcessing;
using static MoneyManager.Application.Write.Tests.ImportTransactionsTests.Data;

namespace MoneyManager.Application.Write.Tests;

public class ImportTransactionsTests
{
    private readonly InMemoryAccountRepository repository;
    private readonly StubbedOfxParser ofxParser;
    private readonly ImportTransactions sut;

    public ImportTransactionsTests()
    {
        this.repository = new InMemoryAccountRepository();
        this.ofxParser = new StubbedOfxParser();
        this.sut = new ImportTransactions(this.repository, this.ofxParser);
    }

    [Fact]
    public async Task Should_track_unknown_account()
    {
        Guid id = Guid.NewGuid();
        this.ofxParser.SetResultFor(TheStream, TheAccountStatement);
        this.repository.NextId = () => id;

        await this.Verify_ImportTransactions(
            TheStream,
            new Account(id, TheAccountStatement.ExternalId, TheAccountStatement.Balance)
        );
    }

    [Fact]
    public async Task Should_synchronize_already_tracked_account()
    {
        Account existingAccount = new(Guid.NewGuid(), TheAccountStatement.ExternalId, 12.34m);
        this.repository.Feed(existingAccount);
        this.ofxParser.SetResultFor(TheStream, TheAccountStatement);

        await this.Verify_ImportTransactions(
            TheStream,
            new Account(existingAccount.Id, existingAccount.ExternalId, TheAccountStatement.Balance)
        );
    }

    private async Task Verify_ImportTransactions(Stream stream, Account expected)
    {
        await this.sut.Execute(stream);

        Account actual = (await this.repository.GetByIdOrDefault(expected.ExternalId))!;
        actual.Should().BeEquivalentTo(expected);
    }

    internal static class Data
    {
        public static readonly MemoryStream TheStream = new(new byte[] { 0xF0, 0x42 });

        public static readonly AccountStatement TheAccountStatement = new(new ExternalId("Bank", "Account"), 1337.42m);
    }
}