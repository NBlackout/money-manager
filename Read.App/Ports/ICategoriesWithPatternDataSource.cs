namespace Read.App.Ports;

public interface ICategoriesWithPatternDataSource
{
    Task<CategoryWithPattern[]> Get();
}

public record CategoryWithPattern(Guid Id, string Label, string Pattern);