using App.Write.Model.ValueObjects;

namespace App.Write.Ports;

public interface ICategorizationRuleImporter
{
    Task<CategorizationRuleToImport[]> Parse(Stream content);
}

public record CategorizationRuleToImport(Label CategoryLabel, string Keywords);