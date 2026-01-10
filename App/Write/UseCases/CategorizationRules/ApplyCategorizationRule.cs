using App.Write.Model.Categories;
using App.Write.Model.CategorizationRules;
using App.Write.Model.ValueObjects;
using App.Write.Ports;

namespace App.Write.UseCases.CategorizationRules;

public class ApplyCategorizationRule(ICategoryRepository categoryRepository, ICategorizationRuleRepository categorizationRuleRepository)
{
    public async Task Execute(CategorizationRuleId categorizationRuleId, CategoryId categoryId, string keywords, Amount? amount)
    {
        Category category = await categoryRepository.By(categoryId);
        CategorizationRule categorizationRule = category.ApplyWhenMatches(categorizationRuleId, keywords, amount);
        await categorizationRuleRepository.Save(categorizationRule);
    }
}