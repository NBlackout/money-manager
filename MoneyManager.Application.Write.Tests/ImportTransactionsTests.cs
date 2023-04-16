using MoneyManager.Application.Write.Model;
using MoneyManager.Application.Write.UseCases;

namespace MoneyManager.Application.Write.Tests;

public class ImportTransactionsTests
{
    private readonly InMemoryAccountRepository accountRepository;
    private readonly StubbedOfxParser ofxParser;
    private readonly ImportTransactions sut;

    public ImportTransactionsTests()
    {
        this.accountRepository = new InMemoryAccountRepository();
        this.ofxParser = new StubbedOfxParser();
        this.sut = new ImportTransactions(this.accountRepository, this.ofxParser);
    }

    [Fact]
    public async Task Should_track_unknown_accounts()
    {
        MemoryStream? memoryStream = new(new byte[] { 0x01, 0x02 });

        const string accountNumber = "Account number";
        this.ofxParser.SetResultFor(memoryStream, new AccountCharacteristics(accountNumber));
        await this.sut.Handle(memoryStream);

        Account? actual = await this.accountRepository.GetByNumber(accountNumber);
        actual.Should().BeEquivalentTo(new Account(accountNumber));
    }

    private class StubbedOfxParser : IOfxParser
    {
        private readonly Dictionary<Stream, AccountCharacteristics> results = new();

        public void SetResultFor(Stream stream, AccountCharacteristics accountCharacteristics) =>
            this.results[stream] = accountCharacteristics;

        public Task<AccountCharacteristics> Process(Stream stream) =>
            Task.FromResult(this.results[stream]);
    }

    private class InMemoryAccountRepository : IAccountRepository
    {
        private readonly Dictionary<string, Account> accountsByNumber = new();

        public Task<Account> GetByNumber(string number) =>
            Task.FromResult(this.accountsByNumber[number]);

        public Task Save(Account account)
        {
            this.accountsByNumber.Add(account.Number, account);

            return Task.CompletedTask;
        }
    }
}