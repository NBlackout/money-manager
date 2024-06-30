namespace Read.App.Ports;

public interface ICategoriesWithKeywordsDataSource
{
    Task<CategoryWithKeywords[]> All();
}

public record CategoryWithKeywords(Guid Id, string Label, string Keywords);
