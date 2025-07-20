namespace App.Read.Ports;

public interface ICategoriesWithKeywordsDataSource
{
    Task<CategoryWithKeywords[]> All();
}

public record CategoryWithKeywords(Guid Id, string Label, string Keywords);