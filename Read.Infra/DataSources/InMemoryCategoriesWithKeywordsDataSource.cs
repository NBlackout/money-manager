﻿namespace Read.Infra.DataSources;

public class InMemoryCategoriesWithKeywordsDataSource(InMemoryCategoryRepository repository)
    : ICategoriesWithKeywordsDataSource
{
    public Task<CategoryWithKeywords[]> All()
    {
        CategoryWithKeywords[] categoriesWithKeywords = repository.Data
            .Where(c => !string.IsNullOrWhiteSpace(c.Keywords))
            .Select(c => new CategoryWithKeywords(c.Id, c.Label, c.Keywords))
            .ToArray();

        return Task.FromResult(categoriesWithKeywords);
    }
}