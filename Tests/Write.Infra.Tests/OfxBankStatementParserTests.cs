using Microsoft.Extensions.DependencyInjection;
using Write.Api;
using Write.Infra.BankStatementParsing;
using Write.Infra.Exceptions;
using static Shared.TestTooling.Resources.Resources;

namespace Write.Infra.Tests;

public sealed class OfxBankStatementParserTests : HostFixture
{
    private readonly OfxBankStatementParser sut;

    public OfxBankStatementParserTests()
    {
        this.sut = this.Resolve<OfxBankStatementParser>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWrite();

    [Fact]
    public async Task Extracts_account_statement()
    {
        AccountStatement expected = new("00012345000", 12345.67m, DateTime.Parse("2023-04-13"),
            new TransactionStatement("TheDebitId", -300.21m, "The debit", DateTime.Parse("2023-04-18"), null),
            new TransactionStatement("TheCreditId", 100.95m, "The credit", DateTime.Parse("2023-04-17"), null)
        );
        await this.Verify(new MemoryStream(OfxSample), expected);
    }

    [Fact]
    public async Task Tells_when_bank_identifier_is_missing()
    {
        await this.Invoking(s => s.Verify_Failure(new MemoryStream(MissingBankIdentifierOfxSample)))
            .Should()
            .ThrowAsync<CannotProcessOfxContent>();
    }

    [Fact]
    public async Task Tells_when_account_number_is_missing()
    {
        await this.Invoking(s => s.Verify_Failure(new MemoryStream(MissingAccountNumberOfxSample)))
            .Should()
            .ThrowAsync<CannotProcessOfxContent>();
    }

    [Fact]
    public async Task Tells_when_balance_is_missing()
    {
        await this.Invoking(s => s.Verify_Failure(new MemoryStream(MissingBalanceOfxSample)))
            .Should()
            .ThrowAsync<CannotProcessOfxContent>();
    }

    private async Task Verify_Failure(Stream stream) =>
        await this.Verify(stream, Randomizer.Any<AccountStatement>());

    private async Task Verify(Stream stream, AccountStatement expected)
    {
        AccountStatement actual = await this.sut.ExtractAccountStatement(stream);
        actual.Should().BeEquivalentTo(expected);
    }
}
