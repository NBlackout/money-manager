using Write.App.Model.Accounts;
using Write.Infra.BankStatementParsing;

namespace Write.Infra.Tests.BankStatementParsing;

public sealed class CsvBankStatementParserTests : HostFixture
{
    private readonly CsvBankStatementParser sut;

    public CsvBankStatementParserTests()
    {
        this.sut = this.Resolve<CsvBankStatementParser>();
    }

    [Fact]
    public async Task Extracts_account_statement()
    {
        await this.Verify(
            CsvSample,
            new AccountStatement(
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
            )
        );
    }

    [Fact]
    public async Task Uses_last_recorded_account_balance_when_missing()
    {
        await this.Verify(
            MissingAccountBalanceOnNewestTransactions,
            new AccountStatement(
                new ExternalId("1234"),
                new Balance(12345.67m, DateOnly.Parse("2020-01-01")),
                new TransactionStatement(
                    new ExternalId("1234_1"),
                    new Amount(-56.78m),
                    new Label("Debit 2"),
                    DateOnly.Parse("2020-01-02"),
                    new Label("Debit parent")
                ),
                new TransactionStatement(
                    new ExternalId("1234_2"),
                    new Amount(-12.34m),
                    new Label("Debit 1"),
                    DateOnly.Parse("2020-01-01"),
                    new Label("Debit parent")
                )
            )
        );
    }

    private async Task Verify(byte[] content, AccountStatement expected)
    {
        AccountStatement actual = await this.sut.ExtractAccountStatement(new MemoryStream(content));
        actual.Should().BeEquivalentTo(expected);
    }
}