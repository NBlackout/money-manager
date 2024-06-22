namespace Client.Read.Infra.Gateways.Category;

public class StubbedCategoryGateway : ICategoryGateway
{
    private CategorySummaryPresentation[] summaries = null!;

    public Task<CategorySummaryPresentation[]> Summaries() =>
        Task.FromResult(this.summaries);

    public void Feed(params CategorySummaryPresentation[] summaries) =>
        this.summaries = summaries;
}