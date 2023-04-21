using Microsoft.Extensions.Hosting;
using MoneyManager.Api.Extensions;
using MoneyManager.Application.Write.Model;
using MoneyManager.Infrastructure.Write.OfxProcessing;
using static MoneyManager.Shared.TestTooling.Resources.Resources;

namespace MoneyManager.Infrastructure.Write.Tests;

public sealed class OfxParserTests : IDisposable
{
    private readonly IHost host;
    private readonly OfxParser sut;

    public OfxParserTests()
    {
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => services.AddWriteDependencies())
            .Build();
        this.sut = this.host.GetRequiredService<IOfxParser, OfxParser>();
    }

    [Fact]
    public async Task Should_extract_account_statement()
    {
        AccountStatement expected = new(new ExternalId("1234567890", "00012345000"), 12345.67m);
        await this.Verify_OfxParser(new MemoryStream(OfxSample), expected);
    }

    [Fact]
    public async Task Should_tell_when_bank_identifier_is_missing()
    {
        await this.Invoking(s => s.Verify_Failure(new MemoryStream(MissingBankIdentifierOfxSample))).Should()
            .ThrowAsync<CannotProcessOfxContent>();
    }

    [Fact]
    public async Task Should_tell_when_account_number_is_missing()
    {
        await this.Invoking(s => s.Verify_Failure(new MemoryStream(MissingAccountNumberOfxSample))).Should()
            .ThrowAsync<CannotProcessOfxContent>();
    }

    [Fact]
    public async Task Should_tell_when_balance_is_missing()
    {
        await this.Invoking(s => s.Verify_Failure(new MemoryStream(MissingBalanceOfxSample))).Should()
            .ThrowAsync<CannotProcessOfxContent>();
    }

    public void Dispose() =>
        this.host.Dispose();

    private async Task Verify_Failure(Stream stream) =>
        await this.Verify_OfxParser(stream, new AccountStatement(new ExternalId("Bank", "Account"), 42.42m));

    private async Task Verify_OfxParser(Stream stream, AccountStatement expected)
    {
        AccountStatement actual = await this.sut.ExtractAccountId(stream);
        actual.Should().Be(expected);
    }
}