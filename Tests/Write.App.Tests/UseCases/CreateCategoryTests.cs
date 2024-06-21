﻿using Write.App.Model.Categories;

namespace Write.App.Tests.UseCases;

public class CreateCategoryTests
{
    private readonly InMemoryCategoryRepository repository = new();
    private readonly CreateCategory sut;

    public CreateCategoryTests()
    {
        this.sut = new CreateCategory(this.repository);
    }

    [Theory, RandomData]
    public async Task Should_create_category(CategorySnapshot expected)
    {
        await this.sut.Execute(expected.Id, expected.Label, expected.Pattern);
        Category actual = await this.repository.By(expected.Id);
        actual.Snapshot.Should().Be(expected);
    }
}