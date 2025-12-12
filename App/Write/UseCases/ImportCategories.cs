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
        Label[] existingLabels = await this.ExistingLabels(categoriesToImport);
        foreach (CategoryToImport categoryToImport in categoriesToImport)
        {
            if (existingLabels.Contains(categoryToImport.Label))
                continue;

            categories.Add(new Category(await categoryRepository.NextIdentity(), categoryToImport.Label, null));
        }

        await categoryRepository.Save(categories.ToArray());
    }

    private async Task<Label[]> ExistingLabels(CategoryToImport[] categoriesToImport)
    {
        Label[] labelsToImport = categoriesToImport.Select(c => c.Label).ToArray();
        Dictionary<Label, Category?> categories = await categoryRepository.By(labelsToImport);

        return categories.Where(kv => kv.Value is not null).Select(kv => kv.Key).ToArray();
    }
}