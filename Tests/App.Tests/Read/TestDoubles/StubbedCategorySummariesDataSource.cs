﻿using App.Read.Ports;

namespace App.Tests.Read.TestDoubles;

public class StubbedCategorySummariesDataSource : ICategorySummariesDataSource
{
    private CategorySummaryPresentation[] data = null!;

    public Task<CategorySummaryPresentation[]> All() =>
        Task.FromResult(this.data);

    public void Feed(CategorySummaryPresentation[] summaries) =>
        this.data = summaries;
}