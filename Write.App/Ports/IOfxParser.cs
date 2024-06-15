namespace Write.App.Ports;

public interface IOfxParser
{
    Task<AccountStatement> ExtractAccountStatement(Stream stream);
}