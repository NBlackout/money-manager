using App.Tests.Write.TestDoubles;
using App.Write.Model.Categories;
using App.Write.Model.CategorizationRules;
using App.Write.Ports;
using App.Write.UseCases;
using Infra.Write.Repositories;

namespace App.Tests.Write.UseCases;

public class ImportCategorizationRulesTests
{
    private static readonly byte[] Content = Any<byte[]>();

    private readonly InMemoryCategorizationRuleRepository categorizationRuleRepository = new();
    private readonly InMemoryCategoryRepository categoryRepository = new();
    private readonly StubbedCategorizationRuleImporter categorizationRuleImporter = new();
    private readonly ImportCategorizationRules sut;

    public ImportCategorizationRulesTests()
    {
        this.sut = new ImportCategorizationRules(this.categorizationRuleRepository, this.categoryRepository, this.categorizationRuleImporter);
    }

    [Theory, RandomData]
    public async Task Imports_many_categorization_rules(
        CategorizationRuleToImport aCategorizationRule,
        CategorizationRuleToImport anotherCategorizationRule,
        CategorySnapshot aCategory,
        CategorySnapshot anotherCategory,
        CategorizationRuleId anId,
        CategorizationRuleId anotherId)
    {
        this.SetImportParsingTo(aCategorizationRule, anotherCategorizationRule);
        this.SetCategoriesTo(
            aCategory with { Label = aCategorizationRule.CategoryLabel.Value },
            anotherCategory with { Label = anotherCategorizationRule.CategoryLabel.Value }
        );
        this.FeedNextIdsTo(anId, anotherId);

        await this.Verify(ExpectedFrom(anId, aCategory, aCategorizationRule), ExpectedFrom(anotherId, anotherCategory, anotherCategorizationRule));
    }

    [Theory, RandomData]
    public async Task Tells_when_no_category_matches(CategorizationRuleToImport categorizationRule, CategorySnapshot category)
    {
        this.SetImportParsingTo(categorizationRule);
        this.SetCategoriesTo(category with { Label = AnythingBut(categorizationRule.CategoryLabel.Value) });
        await this.Verify<CategoryNotFoundException>();
    }

    private async Task Verify(params CategorizationRuleSnapshot[] expected)
    {
        await this.sut.Execute(new MemoryStream(Content));
        this.categorizationRuleRepository.Data.ToArray().Should().Equal(expected);
    }

    private async Task Verify<TException>() where TException : Exception =>
        await this.Invoking(s => s.Verify()).Should().ThrowAsync<TException>();

    private void SetImportParsingTo(params CategorizationRuleToImport[] categorizationRules) =>
        this.categorizationRuleImporter.Feed(new MemoryStream(Content), categorizationRules);

    private void SetCategoriesTo(params CategorySnapshot[] categories) =>
        this.categoryRepository.Feed(categories);

    private void FeedNextIdsTo(params CategorizationRuleId[] ids)
    {
        int nextIdIndex = 0;

        this.categorizationRuleRepository.NextId = () => ids[nextIdIndex++];
    }

    private static CategorizationRuleSnapshot ExpectedFrom(CategorizationRuleId id, CategorySnapshot category, CategorizationRuleToImport categorizationRule) =>
        new(id, category.Id, categorizationRule.Keywords);
}