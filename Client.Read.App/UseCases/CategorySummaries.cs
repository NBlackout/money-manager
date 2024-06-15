namespace Client.Read.App.UseCases;

public class CategorySummaries
{
    private readonly ICategoryGateway gateway;

    public CategorySummaries(ICategoryGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task<CategorySummaryPresentation[]> Execute() =>
        await this.gateway.Summaries();
}