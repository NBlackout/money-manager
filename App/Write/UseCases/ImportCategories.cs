using App.Write.Model.Categories;
using App.Write.Model.ValueObjects;
using App.Write.Ports;

namespace App.Write.UseCases;

public class ImportCategories(ICategoryRepository categoryRepository, ICategoryImporter categoryImporter)
{
    public async Task Execute(Stream content)
    {
        CategoryToImport[] categoriesToImport = await categoryImporter.Parse(content);

        List<Category> categories = [];
        Dictionary<Label, Category?> existingCategories = await this.ExistingCategories(categoriesToImport);
        foreach (CategoryToImport categoryToImport in categoriesToImport.OrderByDescending(c => c.ParentLabel is null))
        {
            if (existingCategories[categoryToImport.Label] is not null)
                continue;

            CategoryId? parentId = IfOf(categoryToImport.ParentLabel, existingCategories);
            Category category = new(await categoryRepository.NextIdentity(), categoryToImport.Label, parentId);
            categories.Add(category);
            existingCategories[categoryToImport.Label] = category;
        }

        await categoryRepository.Save(categories.ToArray());
    }

    private async Task<Dictionary<Label, Category?>> ExistingCategories(CategoryToImport[] categoriesToImport)
    {
        Label[] labels = categoriesToImport.Select(c => c.Label).ToArray();
        Label[] parentLabels = categoriesToImport.Where(c => c.ParentLabel is not null).Select(c => c.ParentLabel!).ToArray();
        Label[] allLabels = labels.Concat(parentLabels).Distinct().ToArray();

        return await categoryRepository.By(allLabels);
    }

    private static CategoryId? IfOf(Label? label, Dictionary<Label, Category?> existingCategories)
    {
        if (label is null)
            return null;
        if (existingCategories[label] is null)
            return null;

        return existingCategories[label]!.Id;
    }
}