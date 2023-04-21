namespace MoneyManager.Application.Write.Ports;

public interface IOfxParser
{
    Task<AccountStatement> ExtractAccountId(Stream stream);
}