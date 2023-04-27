using MoneyManager.Client.Write.Application.UseCases;
using MoneyManager.Client.Write.Infrastructure.AccountGateway;

namespace MoneyManager.Client.Write.Application.Tests;

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