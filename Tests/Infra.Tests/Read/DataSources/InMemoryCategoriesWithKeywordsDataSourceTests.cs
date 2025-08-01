﻿using App.Read.Ports;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;

namespace Infra.Tests.Read.DataSources;

public class InMemoryCategoriesWithKeywordsDataSourceTests : InfraTest<ICategoriesWithKeywordsDataSource, InMemoryCategoriesWithKeywordsDataSource>
{
    private readonly InMemoryCategoryRepository categoryRepository;

    public InMemoryCategoriesWithKeywordsDataSourceTests()
    {
        this.categoryRepository = this.Resolve<ICategoryRepository, InMemoryCategoryRepository>();
    }

    [Theory]
    [RandomData]
    public async Task Gives_categories(CategoryBuilder[] expected)
    {
        this.Feed(expected);
        await this.Verify(expected.Select(c => new CategoryWithKeywords(c.Id, c.Label, c.Keywords)).ToArray());
    }

    [Fact]
    public async Task Excludes_ones_without_keywords()
    {
        this.Feed(Any<CategoryBuilder>() with { Keywords = "" }, Any<CategoryBuilder>() with { Keywords = "   " });
        await this.Verify();
    }

    private async Task Verify(params CategoryWithKeywords[] expected)
    {
        CategoryWithKeywords[] actual = await this.Sut.All();
        actual.Should().Equal(expected);
    }

    private void Feed(params CategoryBuilder[] categories) =>
        this.categoryRepository.Feed([..categories.Select(c => c.ToSnapshot())]);
}