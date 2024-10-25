using Shared.Infra.TestTooling;
using Write.App.Model.Accounts;
using Write.Infra.BankStatementParsing;
using static Shared.TestTooling.Resources.Resources;

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
                "00012345000",
                new Balance(12345.67m, DateOnly.Parse("2023-04-18")),
                new TransactionStatement("00012345000_1", -300.21m, "The debit", DateOnly.Parse("2023-04-18"),
                    "Debit parent"),
                new TransactionStatement("00012345000_2", 100.95m, "The credit", DateOnly.Parse("2023-04-17"),
                    "Credit parent")
            )
        );
    }

    [Fact]
    public async Task Uses_last_recorded_account_balance_when_missing()
    {
        await this.Verify(
            MissingAccountBalanceOnNewestTransactions,
            new AccountStatement(
                "1234",
                new Balance(12345.67m, DateOnly.Parse("2020-01-01")),
                new TransactionStatement("1234_1", -56.78m, "Debit 2", DateOnly.Parse("2020-01-02"), "Debit parent"),
                new TransactionStatement("1234_2", -12.34m, "Debit 1", DateOnly.Parse("2020-01-01"), "Debit parent")
            )
        );
    }

    private async Task Verify(byte[] content, AccountStatement expected)
    {
        AccountStatement actual = await this.sut.ExtractAccountStatement(new MemoryStream(content));
        actual.Should().BeEquivalentTo(expected);
    }
}