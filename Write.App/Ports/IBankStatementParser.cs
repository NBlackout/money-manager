namespace Write.App.Ports;

public interface IBankStatementParser
{
    Task<AccountStatement> Extract(string fileName, Stream stream);
}
