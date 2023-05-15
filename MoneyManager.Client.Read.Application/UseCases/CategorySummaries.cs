namespace MoneyManager.Client.Read.Application.UseCases;

public class CategorySummaries
{
    private readonly ICategoryGateway gateway;

    public CategorySummaries(ICategoryGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task<IReadOnlyCollection<CategorySummaryPresentation>> Execute() =>
        await this.gateway.Summaries();
}