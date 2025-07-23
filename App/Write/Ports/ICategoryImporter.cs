using App.Write.Model.ValueObjects;

namespace App.Write.Ports;

public interface ICategoryImporter
{
    Task<CategoryToImport[]> Parse(Stream content);
}

public record CategoryToImport(Label Label, string Keywords);