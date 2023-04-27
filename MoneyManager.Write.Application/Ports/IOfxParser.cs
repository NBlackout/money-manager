namespace MoneyManager.Write.Application.Ports;

public interface IOfxParser
{
    Task<AccountStatement> ExtractAccountStatement(Stream stream);
}