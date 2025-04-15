using Write.App.Model.Accounts;
using Write.Infra.BankStatementParsing;
using Write.Infra.Exceptions;

namespace Write.Infra.Tests.BankStatementParsing;

public sealed class OfxBankStatementParserTests : InfraFixture<OfxBankStatementParser>
{
    [Fact]
    public async Task Extracts_account_statement()
    {
        AccountStatement expected = new(
            new ExternalId("00012345000"),
            new Balance(12345.67m, DateOnly.Parse("2023-04-13")),
            new TransactionStatement(
                new ExternalId("TheDebitId"),
                new Amount(-300.21m),
                new Label("The debit"),
                DateOnly.Parse("2023-04-18"),
                null
            ),
            new TransactionStatement(
                new ExternalId("TheCreditId"),
                new Amount(100.95m),
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
        await this.Verify<CannotProcessOfxContent>(new MemoryStream(MissingBankIdentifierOfxSample));
    }

    [Fact]
    public async Task Tells_when_account_number_is_missing()
    {
        await this.Verify<CannotProcessOfxContent>(new MemoryStream(MissingAccountNumberOfxSample));
    }

    [Fact]
    public async Task Tells_when_balance_is_missing()
    {
        await this.Verify<CannotProcessOfxContent>(new MemoryStream(MissingBalanceOfxSample));
    }

    private async Task Verify<TException>(Stream stream) where TException : Exception =>
        await this.Invoking(s => s.Verify(stream, Any<AccountStatement>())).Should().ThrowAsync<TException>();

    private async Task Verify(Stream stream, AccountStatement expected)
    {
        AccountStatement actual = await this.Sut.ExtractAccountStatement(stream);
        actual.Should().BeEquivalentTo(expected);
    }
}