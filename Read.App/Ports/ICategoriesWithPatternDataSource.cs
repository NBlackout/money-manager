namespace Read.App.Ports;

public interface ICategoriesWithKeywordsDataSource
{
    Task<CategoryWithKeywords[]> Get();
}

public record CategoryWithKeywords(Guid Id, string Label, string Keywords);