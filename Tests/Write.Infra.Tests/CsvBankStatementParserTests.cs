using Microsoft.Extensions.DependencyInjection;
using Write.Api;
using Write.Infra.BankStatementParsing;
using static Shared.TestTooling.Resources.Resources;

namespace Write.Infra.Tests;

public sealed class CsvBankStatementParserTests : HostFixture
{
    private readonly CsvBankStatementParser sut;

    public CsvBankStatementParserTests()
    {
        this.sut = this.Resolve<CsvBankStatementParser>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWrite();

    [Fact]
    public async Task Extracts_account_statement()
    {
        await this.Verify(
            CsvSample,
            new AccountStatement(
                "00012345000",
                12345.67m,
                DateTime.Parse("2000-01-01"),
                new TransactionStatement("1", -300.21m, "The debit", DateTime.Parse("2023-04-18"), "Debit parent"),
                new TransactionStatement("2", 100.95m, "The credit", DateTime.Parse("2023-04-17"), "Credit parent")
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
                12345.67m,
                DateTime.Parse("2000-01-01"),
                new TransactionStatement("1", -56.78m, "Debit 2", DateTime.Parse("2020-01-02"), "Debit parent"),
                new TransactionStatement("2", -12.34m, "Debit 1", DateTime.Parse("2020-01-01"), "Debit parent")
            )
        );
    }

    private async Task Verify(byte[] content, AccountStatement expected)
    {
        AccountStatement actual = await this.sut.ExtractAccountStatement(new MemoryStream(content));
        actual.Should().BeEquivalentTo(expected);
    }
}