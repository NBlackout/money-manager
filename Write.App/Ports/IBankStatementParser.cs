namespace Write.App.Ports;

public interface IBankStatementParser
{
    Task<AccountStatement> ExtractAccountStatement(Stream stream);
}