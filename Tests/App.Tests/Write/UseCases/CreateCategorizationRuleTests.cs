using App.Write.Model.CategorizationRules;
using App.Write.UseCases;
using Infra.Write.Repositories;

namespace App.Tests.Write.UseCases;

public class CreateCategorizationRuleTests
{
    private readonly InMemoryCategorizationRuleRepository repository = new();
    private readonly CreateCategorizationRule sut;

    public CreateCategorizationRuleTests()
    {
        this.sut = new CreateCategorizationRule(this.repository);
    }

    [Theory]
    [RandomData]
    public async Task Creates_a_new(CategorizationRuleSnapshot categorizationRule) =>
        await this.Verify(categorizationRule);

    private async Task Verify(CategorizationRuleSnapshot expected)
    {
        await this.sut.Execute(expected.Id, expected.CategoryId, expected.Keywords);
        CategorizationRule actual = await this.repository.By(expected.Id);
        actual.Snapshot.Should().Be(expected);
    }
}