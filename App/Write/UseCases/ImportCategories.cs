using App.Write.Model.Categories;
using App.Write.Ports;

namespace App.Write.UseCases;

public class ImportCategories(ICategoryRepository categoryRepository, ICategoryImporter categoryImporter)
{
    public async Task Execute(Stream content)
    {
        CategoryToImport[] categories = await categoryImporter.Parse(content);

        foreach (CategoryToImport category in categories)
            await categoryRepository.Save(new Category(await categoryRepository.NextIdentity(), category.Label, category.Keywords));
    }
}