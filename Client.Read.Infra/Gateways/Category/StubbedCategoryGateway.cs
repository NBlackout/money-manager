namespace Client.Read.Infra.Gateways.Category;

public class StubbedCategoryGateway : ICategoryGateway
{
    private readonly IReadOnlyCollection<CategorySummaryPresentation> summaries;

    public StubbedCategoryGateway(params CategorySummaryPresentation[] summaries)
    {
        this.summaries = summaries;
    }

    public Task<IReadOnlyCollection<CategorySummaryPresentation>> Summaries() =>
        Task.FromResult(this.summaries);
}