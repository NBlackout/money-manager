namespace Write.App.Ports;

public interface IBankStatementParser
{
    Task<AccountStatement> ExtractAccountStatement(string fileName, Stream stream);
}