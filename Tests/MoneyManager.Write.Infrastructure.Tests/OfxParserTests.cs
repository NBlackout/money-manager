using MoneyManager.Write.Infrastructure.OfxProcessing;

namespace MoneyManager.Write.Infrastructure.Tests;

public sealed class OfxParserTests : IDisposable
{
    private readonly IHost host;
    private readonly OfxParser sut;

    public OfxParserTests()
    {
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => services.AddWriteDependencies())
            .Build();
        this.sut = this.host.Service<IOfxParser, OfxParser>();
    }

    [Fact]
    public async Task Should_extract_account_statement()
    {
        AccountStatement expected = new("1234567890", "00012345000", 12345.67m, DateTime.Parse("2023-04-13"),
            new TransactionStatement("TheDebitId"), new TransactionStatement("TheCreditId"));
        await this.Verify_OfxParser(new MemoryStream(Resources.OfxSample), expected);
    }

    [Fact]
    public async Task Should_tell_when_bank_identifier_is_missing()
    {
        await this.Invoking(s => s.Verify_Failure(new MemoryStream(Resources.MissingBankIdentifierOfxSample))).Should()
            .ThrowAsync<CannotProcessOfxContent>();
    }

    [Fact]
    public async Task Should_tell_when_account_number_is_missing()
    {
        await this.Invoking(s => s.Verify_Failure(new MemoryStream(Resources.MissingAccountNumberOfxSample))).Should()
            .ThrowAsync<CannotProcessOfxContent>();
    }

    [Fact]
    public async Task Should_tell_when_balance_is_missing()
    {
        await this.Invoking(s => s.Verify_Failure(new MemoryStream(Resources.MissingBalanceOfxSample))).Should()
            .ThrowAsync<CannotProcessOfxContent>();
    }

    public void Dispose() =>
        this.host.Dispose();

    private async Task Verify_Failure(Stream stream) =>
        await this.Verify_OfxParser(stream,
            new AccountStatement("Bank", "Account", 42.42m, DateTime.Parse("2943-09-26")));

    private async Task Verify_OfxParser(Stream stream, AccountStatement expected)
    {
        AccountStatement actual = await this.sut.ExtractAccountStatement(stream);
        actual.Should().BeEquivalentTo(expected);
    }
}