using App.Write.Model.Categories;
using App.Write.Model.CategorizationRules;
using App.Write.Model.ValueObjects;
using App.Write.UseCases.CategorizationRules;
using Infra.Write.Repositories;

namespace App.Tests.Write.UseCases;

public class ApplyCategorizationRuleTests
{
    private static readonly CategorizationRuleId Id = Any<CategorizationRuleId>();

    private readonly InMemoryCategoryRepository categoryRepository = new();
    private readonly InMemoryCategorizationRuleRepository categorizationRuleRepository = new();
    private readonly ApplyCategorizationRule sut;

    public ApplyCategorizationRuleTests()
    {
        this.sut = new ApplyCategorizationRule(this.categoryRepository, this.categorizationRuleRepository);
    }

    [Theory, RandomData]
    public async Task Applies_generic_rule_on_category(CategorySnapshot category, string keywords)
    {
        this.Feed(category);
        await this.Verify(category.Id, keywords, ExpectedFrom(category, keywords));
    }

    [Theory, RandomData]
    public async Task Applies_exact_amount_rule_on_category(CategorySnapshot category, string keywords, Amount amount)
    {
        this.Feed(category);
        await this.Verify(category.Id, keywords, amount, ExpectedFrom(category, keywords) with { Amount = amount.Value });
    }

    [Theory, RandomData]
    public async Task Applies_approximative_amount_rule_on_category(CategorySnapshot category, string keywords, Amount amount, Amount margin)
    {
        this.Feed(category);
        await this.Verify(category.Id, keywords, amount, margin, ExpectedFrom(category, keywords)with { Amount = amount.Value, Margin = margin.Value });
    }

    private async Task Verify(CategoryId categoryId, string keywords, CategorizationRuleSnapshot expected) =>
        await this.Verify(categoryId, keywords, null, null, expected);

    private async Task Verify(CategoryId categoryId, string keywords, Amount? amount, CategorizationRuleSnapshot expected) =>
        await this.Verify(categoryId, keywords, amount, null, expected);

    private async Task Verify(CategoryId categoryId, string keywords, Amount? amount, Amount? margin, CategorizationRuleSnapshot expected)
    {
        await this.sut.Execute(Id, categoryId, keywords, amount, margin);
        CategorizationRule actual = await this.categorizationRuleRepository.By(Id);
        actual.Snapshot.Should().Be(expected);
    }

    private void Feed(CategorySnapshot category) =>
        this.categoryRepository.Feed(category);

    private static CategorizationRuleSnapshot ExpectedFrom(CategorySnapshot category, string keywords) =>
        new(Id, category.Id, keywords, null, null);
}