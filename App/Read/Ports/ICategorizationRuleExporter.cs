namespace App.Read.Ports;

public interface ICategorizationRuleExporter
{
    Task<Stream> Export(CategorizationRuleSummaryPresentation[] categories);
}