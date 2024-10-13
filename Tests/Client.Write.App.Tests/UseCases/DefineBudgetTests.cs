using Client.Write.Infra.Gateways.Budget;

namespace Client.Write.App.Tests.UseCases;

public class DefineBudgetTests
{
    private readonly SpyBudgetGateway gateway = new();
    private readonly DefineBudget sut;

    public DefineBudgetTests()
    {
        this.sut = new DefineBudget(this.gateway);
    }

    [Theory, RandomData]
    public async Task Defines_budget(Guid id, string name, decimal amount, DateOnly beginDate)
    {
        await this.sut.Execute(id, name, amount, beginDate);
        this.gateway.DefineCalls.Should().Equal((id, name, amount, beginDate));
    }
}