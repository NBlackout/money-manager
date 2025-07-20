namespace App.Read.Ports;

public interface ICategoryExporter
{
    Task<Stream> Export(CategorySummaryPresentation[] categories);
}