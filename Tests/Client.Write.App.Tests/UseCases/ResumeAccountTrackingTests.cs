using Client.Write.Infra.Gateways.Account;

namespace Client.Write.App.Tests.UseCases;

public class ResumeAccountTrackingTests
{
    private readonly SpyAccountGateway gateway = new();
    private readonly ResumeAccountTracking sut;

    public ResumeAccountTrackingTests()
    {
        this.sut = new ResumeAccountTracking(this.gateway);
    }

    [Theory, RandomData]
    public async Task Resumes_account_tracking(Guid id)
    {
        await this.sut.Execute(id);
        this.gateway.ResumeTrackingCalls.Should().Equal(id);
    }
}