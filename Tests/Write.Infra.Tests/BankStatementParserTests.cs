using Microsoft.Extensions.DependencyInjection;
using Write.Api;
using Write.Infra.BankStatementParsing;
using static Shared.TestTooling.Resources.Resources;

namespace Write.Infra.Tests;

public class BankStatementParserTests : HostFixture
{
    private readonly BankStatementParser sut;

    public BankStatementParserTests()
    {
        this.sut = this.Resolve<IBankStatementParser, BankStatementParser>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWrite();

    [Fact]
    public async Task Extracts_ofx_account_statement()
    {
        AccountStatement expected = new("00012345000", 12345.67m, DateOnly.Parse("2023-04-13"),
            new TransactionStatement("TheDebitId", -300.21m, "The debit", DateOnly.Parse("2023-04-18"), null),
            new TransactionStatement("TheCreditId", 100.95m, "The credit", DateOnly.Parse("2023-04-17"), null)
        );
        await this.Verify("sample.ofx", OfxSample, expected);
    }

    [Fact]
    public async Task Extracts_csv_account_statement()
    {
        AccountStatement expected = new("00012345000", 12345.67m, DateOnly.Parse("2000-01-01"),
            new TransactionStatement("1", -300.21m, "The debit", DateOnly.Parse("2023-04-18"), "Debit parent"),
            new TransactionStatement("2", 100.95m, "The credit", DateOnly.Parse("2023-04-17"), "Credit parent")
        );
        await this.Verify("sample.csv", CsvSample, expected);
    }

    private async Task Verify(string fileName, byte[] content, AccountStatement expected)
    {
        AccountStatement actual = await this.sut.Extract(fileName, new MemoryStream(content));
        actual.Should().BeEquivalentTo(expected);
    }
}
