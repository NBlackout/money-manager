using Write.App.Model.Accounts;
using Write.Infra.BankStatementParsing;
using Write.Infra.Exceptions;

namespace Write.Infra.Tests.BankStatementParsing;

public sealed class OfxBankStatementParserTests : HostFixture
{
    private readonly OfxBankStatementParser sut;

    public OfxBankStatementParserTests()
    {
        this.sut = this.Resolve<OfxBankStatementParser>();
    }

    [Fact]
    public async Task Extracts_account_statement()
    {
        AccountStatement expected = new(
            new ExternalId("00012345000"),
            new Balance(12345.67m, DateOnly.Parse("2023-04-13")),
            new TransactionStatement(
                new ExternalId("TheDebitId"),
                -300.21m,
                new Label("The debit"),
                DateOnly.Parse("2023-04-18"),
                null
            ),
            new TransactionStatement(
                new ExternalId("TheCreditId"),
                100.95m,
                new Label("The credit"),
                DateOnly.Parse("2023-04-17"),
                null
            )
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