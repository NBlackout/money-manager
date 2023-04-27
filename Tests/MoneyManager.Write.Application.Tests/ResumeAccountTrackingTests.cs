namespace MoneyManager.Write.Application.Tests;

public class ResumeAccountTrackingTests
{
    [Fact]
    public async Task Should_resume_account_tracking()
    {
        InMemoryAccountRepository repository = new();
        ResumeAccountTracking sut = new(repository);

        AccountSnapshot account = new(Guid.NewGuid(), "Bank", "Number", 34.81m, false);
        repository.Feed(account);

        await sut.Execute(account.Id);

        Account actual = await repository.GetById(account.Id);
        actual.Snapshot.Should().Be(account with { Tracked = true });
    }
}