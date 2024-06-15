using Microsoft.Extensions.DependencyInjection;
using Write.Infra.OfxProcessing;
using static Shared.TestTooling.Resources.Resources;

namespace Write.Infra.Tests;

public sealed class OfxParserTests : HostFixture
{
    private readonly OfxParser sut;

    public OfxParserTests()
    {
        this.sut = this.Resolve<IOfxParser, OfxParser>();
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
        await this.Verify(new MemoryStream(OfxSample), expected);
    }

    [Fact]
    public async Task Should_tell_when_bank_identifier_is_missing()
    {
        await this.Invoking(s => s.Verify_Failure(new MemoryStream(MissingBankIdentifierOfxSample)))
            .Should()
            .ThrowAsync<CannotProcessOfxContent>();
    }

    [Fact]
    public async Task Should_tell_when_account_number_is_missing()
    {
        await this.Invoking(s => s.Verify_Failure(new MemoryStream(MissingAccountNumberOfxSample)))
            .Should()
            .ThrowAsync<CannotProcessOfxContent>();
    }

    [Fact]
    public async Task Should_tell_when_balance_is_missing()
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