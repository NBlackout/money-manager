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
    public async Task Assigns_account_label(AccountSnapshot account, Label newLabel)
    {
        this.Feed(account);
        await this.Verify(account, newLabel);
    }

    private async Task Verify(AccountSnapshot account, Label label)
    {
        await this.sut.Execute(account.Id, label);

        Account actual = await this.repository.By(account.Id);
        actual.Snapshot.Should().Be(account with { Label = label.Value });
    }

    private void Feed(AccountSnapshot account) =>
        this.repository.Feed(account);
}