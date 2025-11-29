using App.Write.Model.Categories;
using App.Write.Model.CategorizationRules;
using App.Write.Ports;

namespace App.Write.UseCases;

public class ApplyCategorizationRule(ICategoryRepository categoryRepository, ICategorizationRuleRepository categorizationRuleRepository)
{
    public async Task Execute(CategorizationRuleId categorizationRuleId, CategoryId categoryId, string keywords)
    {
        Category category = await categoryRepository.By(categoryId);
        CategorizationRule categorizationRule = category.ApplyWhenMatches(categorizationRuleId, keywords);
        await categorizationRuleRepository.Save(categorizationRule);
    }
}