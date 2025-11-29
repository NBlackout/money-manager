using App.Write.Model.Categories;
using App.Write.Model.CategorizationRules;
using App.Write.Ports;

namespace App.Write.UseCases;

public class CreateCategorizationRule(ICategorizationRuleRepository repository)
{
    public async Task Execute(CategorizationRuleId id, CategoryId categoryId, string keywords)
    {
        CategorizationRule categorizationRule = new(id, categoryId, keywords);
        await repository.Save(categorizationRule);
    }
}