namespace Write.App.Tests.UseCases;

public class AssignAccountLabelTests
{
    private readonly InMemoryAccountRepository repository = new();
    private readonly AssignAccountLabel sut;
    private readonly AccountSnapshot existingAccount = Any<AccountSnapshot>();

    public AssignAccountLabelTests()
    {
        this.sut = new AssignAccountLabel(this.repository);
        this.Feed(this.existingAccount);
    }

    [Theory, RandomData]
    public async Task Assigns_account_label(Label label)
    {
        await this.Verify(label);
    }

    private async Task Verify(Label label)
    {
        await this.sut.Execute(this.existingAccount.Id, label);

        Account actual = await this.repository.By(this.existingAccount.Id);
        actual.Snapshot.Should().Be(this.existingAccount with { Label = label.Value });
    }

    private void Feed(AccountSnapshot account) =>
        this.repository.Feed(account);
}