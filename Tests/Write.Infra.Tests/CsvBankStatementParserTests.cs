using Microsoft.Extensions.DependencyInjection;
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
        services.AddWriteDependencies();

    [Fact]
    public async Task Should_extract_account_statement()
    {
        AccountStatement expected = new("1234567890", "00012345000", 12345.67m, DateTime.Parse("2000-01-01"),
            new TransactionStatement("1", -300.21m, "The debit", DateTime.Parse("2023-04-18"), "Debit parent"),
            new TransactionStatement("2", 100.95m, "The credit", DateTime.Parse("2023-04-17"), "Credit parent")
        );
        await this.Verify(new MemoryStream(CsvSample), expected);
    }

    private async Task Verify(Stream stream, AccountStatement expected)
    {
        AccountStatement actual = await this.sut.ExtractAccountStatement(stream);
        actual.Should().BeEquivalentTo(expected);
    }
}
