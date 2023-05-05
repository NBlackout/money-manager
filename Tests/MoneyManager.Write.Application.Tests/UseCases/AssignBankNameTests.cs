namespace MoneyManager.Write.Application.Tests.UseCases;

public class AssignBankNameTests
{
    private readonly InMemoryBankRepository repository;
    private readonly AssignBankName sut;

    public AssignBankNameTests()
    {
        this.repository = new InMemoryBankRepository();
        this.sut = new AssignBankName(this.repository);
    }

    [Fact]
    public async Task Should_test_name()
    {
        Bank bank = (BankBuilder.For(Guid.NewGuid()) with { Name = "Previous name" }).Build();
        this.repository.Feed(bank);

        const string newName = "The new and much better name";
        await this.sut.Execute(bank.Id, newName);

        Bank actual = await this.repository.GetById(bank.Id);
        actual.Snapshot.Should().Be(bank.Snapshot with { Name = newName });
    }
}