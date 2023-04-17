namespace MoneyManager.Application.Write.Ports;

public interface IOfxProcessor
{
    Task<AccountIdentification> Parse(Stream stream);
}