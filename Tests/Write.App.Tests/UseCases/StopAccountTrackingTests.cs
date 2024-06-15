namespace Write.App.Tests.UseCases;

public class StopAccountTrackingTests
{
    private readonly InMemoryAccountRepository repository = new();
    private readonly StopAccountTracking sut;

    public StopAccountTrackingTests()
    {
        this.sut = new StopAccountTracking(this.repository);
    }

    [Theory, RandomData]
    public async Task Should_resume_account_tracking(AccountBuilder account)
    {
        this.repository.Feed(account.Build());

        await this.sut.Execute(account.Id);

        Account actual = await this.repository.By(account.Id);
        actual.Snapshot.Should().Be(account.Build().Snapshot with { Tracked = false });
    }
}