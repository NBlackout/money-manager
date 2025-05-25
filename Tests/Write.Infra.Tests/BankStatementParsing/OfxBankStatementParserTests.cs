using Write.App.Model.Accounts;
using Write.Infra.BankStatementParsing;
using Write.Infra.Exceptions;

namespace Write.Infra.Tests.BankStatementParsing;

public class OfxBankStatementParserTests : InfraTest<OfxBankStatementParser>
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
        await this.Verify(OfxSample, expected);
    }

    [Fact]
    public async Task Tells_when_bank_identifier_is_missing()
    {
        await this.Verify<CannotProcessOfxContent>(MissingBankIdentifierOfxSample);
    }

    [Fact]
    public async Task Tells_when_account_number_is_missing()
    {
        await this.Verify<CannotProcessOfxContent>(MissingAccountNumberOfxSample);
    }

    [Fact]
    public async Task Gives_ledger_balance_when_available_is_missing()
    {
        await this.Verify(
            MissingAvailableBalanceOfxSample,
            new AccountStatement(new ExternalId("00012345000"), new Balance(12345.67m, DateOnly.Parse("2023-04-13")))
        );
    }

    [Fact]
    public async Task Sanitizes_non_xml_headers()
    {
        await this.Verify(
            NonXmlHeadersOfxSample,
            new AccountStatement(new ExternalId("00012345000"), new Balance(12345.67m, DateOnly.Parse("2023-04-13")))
        );
    }

    private async Task Verify<TException>(byte[] content) where TException : Exception =>
        await this.Invoking(s => s.Verify(content, Any<AccountStatement>())).Should().ThrowAsync<TException>();

    private async Task Verify(byte[] content, AccountStatement expected)
    {
        AccountStatement actual = await this.Sut.ExtractAccountStatement(new MemoryStream(content));
        actual.Should().BeEquivalentTo(expected);
    }
}