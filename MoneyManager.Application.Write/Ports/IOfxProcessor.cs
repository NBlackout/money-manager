namespace MoneyManager.Application.Write.Ports;

public interface IOfxProcessor
{
    Task<AccountId> Parse(Stream stream);
}