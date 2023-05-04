using MoneyManager.Client.Write.Infrastructure.AccountGateway;

namespace MoneyManager.Client.Write.Application.Tests;

public class ResumeAccountTrackingTests
{
    private readonly SpyAccountGateway gateway;
    private readonly ResumeAccountTracking sut;

    public ResumeAccountTrackingTests()
    {
        this.gateway = new SpyAccountGateway();
        this.sut = new ResumeAccountTracking(this.gateway);
    }

    [Fact]
    public async Task Should_resume_account_tracking()
    {
        Guid id = Guid.NewGuid();

        await this.sut.Execute(id);

        this.gateway.ResumeTrackingCalls.Should().Equal(id);
    }
}