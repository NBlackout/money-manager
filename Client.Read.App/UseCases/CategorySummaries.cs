namespace Client.Read.App.UseCases;

public class CategorySummaries(ICategoryGateway gateway)
{
    public async Task<CategorySummaryPresentation[]> Execute() =>
        await gateway.Summaries();
}