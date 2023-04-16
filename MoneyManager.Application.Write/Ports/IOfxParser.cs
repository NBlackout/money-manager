namespace MoneyManager.Application.Write.Ports;

public interface IOfxParser
{
    Task<AccountCharacteristics> Process(Stream stream);
}