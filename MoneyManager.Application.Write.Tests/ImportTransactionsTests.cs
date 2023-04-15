using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using MoneyManager.Application.Write.Model;
using MoneyManager.Application.Write.Ports;
using Xunit;

namespace MoneyManager.Application.Write.Tests;

public class ImportTransactionsTests
{
    private readonly InMemoryAccountRepository accountRepository;
    private readonly StubbedOfxParser ofxParser;
    private readonly ImportTransactions sut;

    public ImportTransactionsTests()
    {
        accountRepository = new InMemoryAccountRepository();
        ofxParser = new StubbedOfxParser();
        sut = new ImportTransactions(accountRepository, ofxParser);
    }

    [Fact]
    public async Task Should_track_unknown_accounts()
    {
        var memoryStream = new MemoryStream(new byte[] { 0x01, 0x02 });

        const string accountNumber = "Account number";
        ofxParser.SetResultFor(memoryStream, new AccountCharacteristics(accountNumber));
        await sut.Handle(memoryStream);

        var actual = await accountRepository.GetByNumber(accountNumber);
        actual.Should().BeEquivalentTo(new Account(accountNumber));
    }

    private class StubbedOfxParser : IOfxParser
    {
        private readonly Dictionary<Stream, AccountCharacteristics> results = new();

        public void SetResultFor(Stream stream, AccountCharacteristics accountCharacteristics) =>
            this.results[stream] = accountCharacteristics;

        public Task<AccountCharacteristics> Process(Stream stream) =>
            Task.FromResult(results[stream]);
    }

    private class InMemoryAccountRepository : IAccountRepository
    {
        private readonly Dictionary<string, Account> accountsByNumber = new();

        public Task<Account> GetByNumber(string number) =>
            Task.FromResult(accountsByNumber[number]);

        public Task Save(Account account)
        {
            accountsByNumber.Add(account.Number, account);

            return Task.CompletedTask;
        }
    }
}