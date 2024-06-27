using Microsoft.Extensions.DependencyInjection;
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
        services.AddWriteDependencies();

    [Fact]
    public async Task Should_extract_account_statement()
    {
        AccountStatement expected = new("1234567890", "00012345000", 12345.67m, DateTime.Parse("2023-04-13"),
            new TransactionStatement("TheDebitId", -300.21m, "The debit", DateTime.Parse("2023-04-18")),
            new TransactionStatement("TheCreditId", 100.95m, "The credit", DateTime.Parse("2023-04-17"))
        );
        AccountStatement actual = await this.sut.ExtractAccountStatement(new MemoryStream(OfxSample));
        actual.Should().BeEquivalentTo(expected);
    }
}