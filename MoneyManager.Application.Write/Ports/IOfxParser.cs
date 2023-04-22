namespace MoneyManager.Application.Write.Ports;

public interface IOfxParser
{
    Task<AccountStatement> ExtractAccountStatement(Stream stream);
}