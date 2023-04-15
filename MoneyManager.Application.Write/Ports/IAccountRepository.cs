using MoneyManager.Application.Write.Model;

namespace MoneyManager.Application.Write.Ports;

public interface IAccountRepository
{
    Task<Account> GetByNumber(string number);
    Task Save(Account account);
}