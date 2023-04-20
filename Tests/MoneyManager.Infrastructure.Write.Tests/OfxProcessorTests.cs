using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneyManager.Api.Extensions;
using MoneyManager.Application.Write.Model;
using MoneyManager.Infrastructure.Write.OfxProcessor;
using MoneyManager.Shared.TestTooling;

namespace MoneyManager.Infrastructure.Write.Tests;

public sealed class OfxProcessorTests : IDisposable
{
    private readonly IHost host;
    private readonly OfxProcessor.OfxProcessor sut;

    public OfxProcessorTests()
    {
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => services.AddWriteDependencies())
            .Build();
        this.sut = (OfxProcessor.OfxProcessor)this.host.Services.GetRequiredService<IOfxProcessor>();
    }

    [Fact]
    public async Task Should_extract_account_identification()
    {
        using MemoryStream ofxSample = new(Resources.OfxSample);
        AccountId actual = await this.sut.Parse(ofxSample);
        actual.Should().Be(new AccountId("1234567890", "00012345000"));
    }

    [Fact]
    public async Task Should_tell_when_bank_identifier_is_missing()
    {
        MemoryStream invalidOfxContent = new(Resources.MissingBankIdentifierOfxSample);
        await this.sut.Invoking(s => s.Parse(invalidOfxContent)).Should().ThrowAsync<CannotProcessOfxContent>();
        await invalidOfxContent.DisposeAsync();
    }

    [Fact]
    public async Task Should_tell_when_account_number_is_missing()
    {
        MemoryStream invalidOfxContent = new(Resources.MissingAccountNumberOfxSample);
        await this.sut.Invoking(s => s.Parse(invalidOfxContent)).Should().ThrowAsync<CannotProcessOfxContent>();
        await invalidOfxContent.DisposeAsync();
    }

    public void Dispose() =>
        this.host.Dispose();
}