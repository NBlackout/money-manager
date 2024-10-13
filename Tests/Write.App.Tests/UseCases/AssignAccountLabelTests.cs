using static Shared.TestTooling.Randomizer;

namespace Write.App.Tests.UseCases;

public class AssignAccountLabelTests
{
    private readonly InMemoryAccountRepository repository = new();
    private readonly AssignAccountLabel sut;

    public AssignAccountLabelTests()
    {
        this.sut = new AssignAccountLabel(this.repository);
    }

    [Theory, RandomData]
    public async Task Assigns_account_label(AccountSnapshot account)
    {
        this.repository.Feed(account);

        string newLabel = Another(account.Label);
        await this.sut.Execute(account.Id, newLabel);

        Account actual = await this.repository.By(account.Id);
        actual.Snapshot.Should().Be(account with { Label = newLabel });
    }
}