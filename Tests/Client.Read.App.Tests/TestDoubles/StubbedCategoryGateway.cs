namespace Client.Read.App.Tests.TestDoubles;

public class StubbedCategoryGateway : ICategoryGateway
{
    private CategorySummaryPresentation[] summaries = null!;

    public Task<CategorySummaryPresentation[]> Summaries() =>
        Task.FromResult(this.summaries);

    public void Feed(params CategorySummaryPresentation[] summaries) =>
        this.summaries = summaries;
}