using Write.App.Model.Accounts;
using Write.Infra.BankStatementParsing;

namespace Write.Infra.Tests.BankStatementParsing;

public class BankStatementParserTests : InfraTest<IBankStatementParser, BankStatementParser>
{
    [Fact]
    public async Task Extracts_ofx_account_statement()
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
        await this.Verify("sample.ofx", OfxSample, expected);
    }

    [Fact]
    public async Task Extracts_csv_account_statement()
    {
        AccountStatement expected = new(
            new ExternalId("00012345000"),
            new Balance(12345.67m, DateOnly.Parse("2023-04-18")),
            new TransactionStatement(
                new ExternalId("00012345000_1"),
                new Amount(-300.21m),
                new Label("The debit"),
                DateOnly.Parse("2023-04-18"),
                new Label("Debit parent")
            ),
            new TransactionStatement(
                new ExternalId("00012345000_2"),
                new Amount(100.95m),
                new Label("The credit"),
                DateOnly.Parse("2023-04-17"),
                new Label("Credit parent")
            )
        );
        await this.Verify("sample.csv", CsvSample, expected);
    }

    private async Task Verify(string fileName, byte[] content, AccountStatement expected)
    {
        AccountStatement actual = await this.Sut.Extract(fileName, new MemoryStream(content));
        actual.Should().BeEquivalentTo(expected);
    }
}