using MoneyManager.Application.Write.Model;
using MoneyManager.Application.Write.UseCases;
using MoneyManager.Infrastructure.Write.OfxProcessor;
using MoneyManager.Infrastructure.Write.Repositories;
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
        this.repository.SetNextIdentity(NextIdentity);
        this.ofxProcessor.SetResultFor(OfxStream, new AccountIdentification(AnAccountNumber));

        await this.sut.Execute(OfxStream);

        Account actual = await this.repository.GetById(NextIdentity);
        actual.Should().BeEquivalentTo(new Account(NextIdentity, AnAccountNumber));
    }

    [Fact]
    public async Task Should_skip_already_tracked_account()
    {
        Account existing = new(Guid.NewGuid(), AnAccountNumber);

        this.repository.MarkAsExisting(AnAccountNumber);
        this.repository.Feed(existing);
        this.ofxProcessor.SetResultFor(OfxStream, new AccountIdentification(AnAccountNumber));

        await this.sut.Execute(OfxStream);

        Account actual = await this.repository.GetById(existing.Id);
        actual.Should().BeEquivalentTo(existing);
    }


    internal static class Data
    {
        public static readonly Guid NextIdentity = Guid.Parse("D9223774-1E7F-440F-A9F3-FA05DBC2E81A");
        public static readonly MemoryStream OfxStream = new(new byte[] { 0x01, 0x02 });
        public const string AnAccountNumber = "Account number";
    }
}