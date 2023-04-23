using MoneyManager.Client.Application.Write.UseCases;
using MoneyManager.Client.Infrastructure.Write.AccountGateway;

namespace MoneyManager.Client.Application.Write.Tests;

public class ResumeAccountTrackingTests
{
    [Fact]
    public async Task Should_resume_account_tracking()
    {
        StubbedAccountGateway gateway = new();
        ResumeAccountTracking sut = new(gateway);

        Guid id = Guid.NewGuid();
        await sut.Execute(id);

        gateway.ResumeTrackingCalls.Should().Equal(id);
    }
}