using App.Write.Model.Categories;
using App.Write.Model.CategorizationRules;
using App.Write.Model.ValueObjects;
using App.Write.Ports;

namespace App.Write.UseCases;

public class ImportCategorizationRules(
    ICategorizationRuleRepository categorizationRuleRepository,
    ICategoryRepository categoryRepository,
    ICategorizationRuleImporter categorizationRuleImporter
)
{
    public async Task Execute(Stream content)
    {
        CategorizationRuleToImport[] categorizationRulesToImport = await this.Parse(content);
        Dictionary<Label, Category?> categories = await this.CategoriesOf(categorizationRulesToImport);
        CategorizationRule[] categorizationRules = await this.Match(categorizationRulesToImport, categories);
        await this.Save(categorizationRules);
    }

    private async Task<CategorizationRuleToImport[]> Parse(Stream content) =>
        await categorizationRuleImporter.Parse(content);

    private async Task<Dictionary<Label, Category?>> CategoriesOf(CategorizationRuleToImport[] categorizationRules) =>
        await categoryRepository.By(categorizationRules.Select(r => r.CategoryLabel).ToArray());

    private async Task<CategorizationRule[]> Match(CategorizationRuleToImport[] categorizationRulesToImport, Dictionary<Label, Category?> categories)
    {
        List<CategorizationRule> categorizationRules = [];

        foreach (CategorizationRuleToImport categorizationRuleToImport in categorizationRulesToImport)
        {
            Category? category = categories[categorizationRuleToImport.CategoryLabel];
            if (category is null)
                throw new CategoryNotFoundException();

            CategorizationRuleId id = await categorizationRuleRepository.NextIdentity();
            CategorizationRule categorizationRule = category.ApplyWhenMatches(id, categorizationRuleToImport.Keywords);
            categorizationRules.Add(categorizationRule);
        }

        return categorizationRules.ToArray();
    }

    private async Task Save(CategorizationRule[] categorizationRules)
    {
        foreach (CategorizationRule categorizationRule in categorizationRules)
            await categorizationRuleRepository.Save(categorizationRule);
    }
}